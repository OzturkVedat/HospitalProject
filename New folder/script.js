document.addEventListener('DOMContentLoaded', function () {
  document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
      e.preventDefault();

      const targetSectionId = this.getAttribute('href').substring(1);

      document.querySelectorAll('section').forEach(section => {
        section.style.display = 'none';
      });

      document.getElementById(targetSectionId).style.display = 'flex';

      document.getElementById(targetSectionId).scrollIntoView({
        behavior: 'smooth'
      });
    });
  });
});
function handleButtonClick(buttonName, doctorId) {
  alert(buttonName + ' clicked for Doctor ID ' + doctorId);
  // Add your logic here to handle button clicks for each row
}
