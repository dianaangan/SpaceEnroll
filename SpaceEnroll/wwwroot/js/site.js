const starsContainer = document.getElementById('stars');

// Function to create stars
function createStars(numStars) {
    for (let i = 0; i < numStars; i++) {
        const star = document.createElement('div');
        const size = Math.random() * 3 + 1; // Size between 1 and 4
        const left = Math.random() * 100; // Position from 0 to 100%
        const top = Math.random() * 100; // Position from 0 to 100%

        // Apply styles
        star.classList.add('star');
        star.style.width = `${size}px`;
        star.style.height = `${size}px`;
        star.style.left = `${left}%`;
        star.style.top = `${top}%`;

        // Animation duration based on size
        const animationDuration = Math.random() * 5 + 5; // Between 5 and 10 seconds
        star.style.animation = `moveStar ${animationDuration}s linear infinite`;

        // Add star to the container
        starsContainer.appendChild(star);
    }
}

createStars(500);