document.addEventListener("DOMContentLoaded", () => {
    const fileInput = document.getElementById("employee-image-input");
    const previewImage = document.getElementById("upload-preview-image");
    const previewPlaceholder = document.getElementById("upload-preview-placeholder");
    const fileName = document.getElementById("upload-file-name");

    if (!fileInput || !previewImage || !previewPlaceholder || !fileName) {
        return;
    }

    fileInput.addEventListener("change", (event) => {
        const input = event.target;
        if (!(input instanceof HTMLInputElement) || !input.files || input.files.length === 0) {
            previewImage.classList.add("is-hidden");
            previewImage.removeAttribute("src");
            previewPlaceholder.classList.remove("is-hidden");
            fileName.textContent = "No file chosen";
            return;
        }

        const selectedFile = input.files[0];
        fileName.textContent = selectedFile.name;

        const reader = new FileReader();
        reader.onload = ({ target }) => {
            if (typeof target?.result !== "string") {
                return;
            }

            previewImage.src = target.result;
            previewImage.classList.remove("is-hidden");
            previewPlaceholder.classList.add("is-hidden");
        };

        reader.readAsDataURL(selectedFile);
    });
});
