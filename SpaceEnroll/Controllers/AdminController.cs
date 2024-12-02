using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceEnroll.Data;
using SpaceEnroll.Models;
using SpaceEnroll.Models.Entities;
using System.Text.RegularExpressions;

namespace SpaceEnroll.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public AdminController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Enrollment()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetSubjectsWithSchedule()
        {
            try
            {
                var subjectsWithSchedule = dbContext.Subjects
                    .Join(dbContext.Schedules,
                        subject => subject.SubjectCode,
                        schedule => schedule.SubjectCode,
                        (subject, schedule) => new
                        {
                            edpCode = subject.SubjectCode,
                            subject = subject.Description,
                            units = subject.Units,
                            days = schedule.Days,
                            time = $"{schedule.StartTime.ToString("hh:mm tt")} - {schedule.EndTime.ToString("hh:mm tt")}"
                        }).ToList();

                return Json(subjectsWithSchedule);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine(ex.Message);
                return Json(new { error = "Failed to load subjects" });
            }
        }
 


    public IActionResult StudentEdit()
        {
            return View();
        }
        public IActionResult SubjectEdit()
        {
            return View();
        }

        public IActionResult ScheduleEdit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentAdd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SubjectAdd()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ScheduleAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StudentAdd(AddStudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await dbContext.Students.AnyAsync(s => s.StudentId == viewModel.StudentId))
                {
                    TempData["ErrorMessage"] = "Student ID already exists.";
                    return View(viewModel);
                }

                if (!IsNumber(viewModel.StudentId))
                {
                    TempData["ErrorMessage"] = "Student ID must be a number.";
                    return View(viewModel);
                }

                if (!IsLetters(viewModel.FirstName) || !IsLetters(viewModel.LastName))
                {
                    TempData["ErrorMessage"] = "First name and last name must contain only letters.";
                    return View(viewModel);
                }

                if (await dbContext.Students.AnyAsync(s => s.Email == viewModel.Email))
                {
                    TempData["ErrorMessage"] = "Email already exists.";
                    return View(viewModel);
                }

                var student = new Student
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Email = viewModel.Email,
                    StudentId = viewModel.StudentId,
                    Course = viewModel.Course,
                    Year = viewModel.Year
                };

                await dbContext.Students.AddAsync(student);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("StudentList", "Admin");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubjectAdd(AddSubjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (await dbContext.Subjects.AnyAsync(s => s.SubjectCode == viewModel.SubjectCode))
                {
                    TempData["ErrorMessage"] = "Subject Code already exists.";
                    return View(viewModel);
                }

                if (!IsNumber(viewModel.SubjectCode))
                {
                    TempData["ErrorMessage"] = "Subject code must be a number.";
                    return View(viewModel);
                }

                var subject = new Subject
                {
                    SubjectCode = viewModel.SubjectCode,
                    Description = viewModel.Description,
                    Category = viewModel.Category,
                    Units = viewModel.Units,
                    Offering = viewModel.Offering,
                    CourseCode = viewModel.CourseCode,
                    CurriculumYear = viewModel.CurriculumYear
                };

                await dbContext.Subjects.AddAsync(subject);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("SubjectList", "Admin");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleAdd(AddScheduleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!await dbContext.Subjects.AnyAsync(s => s.SubjectCode == viewModel.SubjectCode))
                {
                    TempData["ErrorMessage"] = "The subject code does not exist.";
                    return View(viewModel);
                }

                if (await dbContext.Schedules.AnyAsync(s => s.SubjectCode == viewModel.SubjectCode))
                {
                    TempData["ErrorMessage"] = "Schedule already exists.";
                    return View(viewModel);
                }
                var startTime = viewModel.StartTime.TimeOfDay;
                var endTime = viewModel.EndTime.TimeOfDay;

                if (startTime < new TimeSpan(7, 30, 0) || startTime > new TimeSpan(21, 30, 0))
                {
                    TempData["ErrorMessage"] = "Start time must be between 7:30 AM and 9:30 PM.";
                    return View(viewModel);
                }

                if (endTime < new TimeSpan(7, 30, 0) || endTime > new TimeSpan(21, 30, 0))
                {
                    TempData["ErrorMessage"] = "End time must be between 7:30 AM and 9:30 PM.";
                    return View(viewModel);
                }

                if (startTime >= endTime)
                {
                    TempData["ErrorMessage"] =  "End time must be after the start time.";
                    return View(viewModel);
                }

                var schedule = new Schedule
                {
                    SubjectCode = viewModel.SubjectCode,
                    StartTime = viewModel.StartTime,
                    EndTime = viewModel.EndTime,
                    Days = viewModel.Days,
                    Section = viewModel.Section,
                    Room = viewModel.Room,
                    SchoolYear = viewModel.SchoolYear
                };

                await dbContext.Schedules.AddAsync(schedule);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("ScheduleList", "Admin");
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StudentList()
        {
            var students = await dbContext.Students.ToListAsync();
            return View(students);
        }


        [HttpGet]
        public async Task<IActionResult> SubjectList()
        {
                var subjects = await dbContext.Subjects.ToListAsync();
                return View(subjects);
            
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleList()
        {
            var schedules = await dbContext.Schedules.ToListAsync();
            return View(schedules);

        }

        [HttpGet]
        public async Task<IActionResult> StudentEdit(string StudentId)
        {
            var student = await dbContext.Students.FindAsync(StudentId);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> SubjectEdit(string SubjectCode)
        {
            var subject = await dbContext.Subjects.FindAsync(SubjectCode);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleEdit(string SubjectCode)
        {
            var schedule = await dbContext.Schedules.FindAsync(SubjectCode);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> StudentEdit(Student viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel); 
            }

            var student = await dbContext.Students.FindAsync(viewModel.StudentId);
            if (student == null)
            {
                return NotFound();
            }

            if (await dbContext.Students.AnyAsync(s => s.Email == viewModel.Email && s.StudentId != student.StudentId))
            {
                TempData["ErrorMessage"] = "Email already exists.";
                return View(viewModel);
            }

            student.FirstName = viewModel.FirstName;
            student.LastName = viewModel.LastName;
            student.Email = viewModel.Email;
            student.Course = viewModel.Course;
            student.Year = viewModel.Year;

            await dbContext.SaveChangesAsync();
            return RedirectToAction("StudentList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> SubjectEdit(Subject viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var subject = await dbContext.Subjects.FindAsync(viewModel.SubjectCode);

            if (subject == null)
            {
                return NotFound();
            }

            subject.Description = viewModel.Description;
            subject.Category = viewModel.Category;
            subject.Units = viewModel.Units;
            subject.Offering = viewModel.Offering;
            subject.CourseCode = viewModel.CourseCode;
            subject.CurriculumYear = viewModel.CurriculumYear;

            await dbContext.SaveChangesAsync();
            return RedirectToAction("SubjectList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ScheduletEdit(Schedule viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var schedule = await dbContext.Schedules.FindAsync(viewModel.SubjectCode);

            if (schedule == null)
            {
                return NotFound();
            }

            var startTime = viewModel.StartTime.TimeOfDay;
            var endTime = viewModel.EndTime.TimeOfDay;

            if (startTime < new TimeSpan(7, 30, 0) || startTime > new TimeSpan(21, 30, 0))
            {
                TempData["ErrorMessage"] = "Start time must be between 7:30 AM and 9:30 PM.";
                return View(viewModel);
            }

            if (startTime >= endTime)
            {
                TempData["ErrorMessage"] = "End time must be after the start time.";
                return View(viewModel);
            }

            schedule.StartTime = viewModel.StartTime;
            schedule.EndTime = viewModel.EndTime;
            schedule.Days = viewModel.Days;
            schedule.Section = viewModel.Section;
            schedule.Room = viewModel.Room;
            schedule.SchoolYear = viewModel.SchoolYear;

            await dbContext.SaveChangesAsync();
            return RedirectToAction("ScheduleList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> StudentDelete(string StudentId)
        {
            var student = await dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StudentId == StudentId);

            if (student != null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("StudentList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> SubjectDelete(string SubjectCode)
        {
            var subject = await dbContext.Subjects
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectCode == SubjectCode);

            if (subject != null)
            {
                dbContext.Subjects.Remove(subject);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("SubjectList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleDelete(string SubjectCode)
        {
            var schedule = await dbContext.Schedules
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubjectCode == SubjectCode);

            if (schedule != null)
            {
                dbContext.Schedules.Remove(schedule);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("ScheduleList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> StudentDeleteMultiple(List<string> studentIds)
        {
            if (studentIds == null || !studentIds.Any())
            {
                return RedirectToAction("StudentList", "Admin");
            }

            var studentsToDelete = await dbContext.Students
                .Where(s => studentIds.Contains(s.StudentId))
                .ToListAsync();

            if (studentsToDelete.Any())
            {
                dbContext.Students.RemoveRange(studentsToDelete);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("StudentList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> SubjectDeleteMultiple(List<string> subjectCodes)
        {
            if (subjectCodes == null || !subjectCodes.Any())
            {
                return RedirectToAction("SubjectList", "Admin");
            }

            var subjectsToDelete = await dbContext.Subjects
                .Where(s => subjectCodes.Contains(s.SubjectCode))
                .ToListAsync();

            if (subjectsToDelete.Any())
            {
                dbContext.Subjects.RemoveRange(subjectsToDelete);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("SubjectList", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleDeleteMultiple(List<string> subjectCodes)
        {
            if (subjectCodes == null || !subjectCodes.Any())
            {
                return RedirectToAction("ScheduletList", "Admin");
            }

            var schedulesToDelete = await dbContext.Schedules
                .Where(s => subjectCodes.Contains(s.SubjectCode))
                .ToListAsync();

            if (schedulesToDelete.Any())
            {
                dbContext.Schedules.RemoveRange(schedulesToDelete);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("ScheduleList", "Admin");
        }

        private bool IsNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        private bool IsLetters(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }

        

        // Action to fetch student info based on StudentId
        [HttpGet]
        public JsonResult GetStudentInfo(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return Json(new { success = false, message = "Student ID is required." });
            }

            // Query the database for the student
            var student = dbContext.Students.FirstOrDefault(s => s.StudentId == studentId);

            if (student != null)
            {
                return Json(new
                {
                    success = true,
                    student = new
                    {
                        student.StudentId,
                        student.FirstName,
                        student.LastName,
                        student.Email,
                        student.Course,
                        student.Year
                    }
                });
            }

            return Json(new { success = false, message = "Student not found." });
        }
    }


}
