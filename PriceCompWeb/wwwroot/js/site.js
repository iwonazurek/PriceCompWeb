document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("form[data-confirm]").forEach((form) => {
    form.addEventListener("submit", (event) => {
      const message = form.getAttribute("data-confirm") || "Potwierdz akcje?";
      if (!window.confirm(message)) {
        event.preventDefault();
      }
    });
  });

  const sampleButton = document.querySelector("[data-sample-basket]");
  const basketText = document.querySelector("#ItemsText");

  if (sampleButton && basketText) {
    sampleButton.addEventListener("click", () => {
      basketText.value = "mleko\nchleb\nmaslo";
      basketText.focus();
    });
  }
});
