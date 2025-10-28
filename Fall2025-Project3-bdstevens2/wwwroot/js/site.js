// Function defined outside
function setupCustomFileInputs() {
    console.log("setupCustomFileInputs function started."); // Log: Function called

    // Use a more specific selector targeting the hidden inputs
    $('input[type="file"].input-file-hidden').each(function () {
        const inputFile = $(this);
        const inputId = inputFile.attr('id');
        console.log("Found file input with ID:", inputId); // Log: Found an input

        // Ensure the ID exists before proceeding
        if (!inputId) {
            console.error("File input found, but it is missing an ID. Cannot attach label functionality.");
            return; // Skip this input if it has no ID
        }

        // Find the label specifically targeting this input's ID
        const fileLabel = $(`label[for='${inputId}'].btn-gold-file`);

        if (fileLabel.length === 0) {
            console.warn("Could not find the custom button label for input ID:", inputId); // Log: Label not found
        } else {
            console.log("Found label for input ID:", inputId); // Log: Label found
        }

        // Find or create the span to display the filename
        let displaySpan = inputFile.next('.file-name-display');
        if (displaySpan.length === 0) {
            console.log("Creating display span for input ID:", inputId); // Log: Creating span
            displaySpan = $('<span class="file-name-display" style="margin-left: 10px; color: #adbac7;"></span>');
            // Insert the span *after* the label button for better visual placement
            fileLabel.after(displaySpan);
        } else {
            console.log("Found existing display span for input ID:", inputId); // Log: Found span
        }

        // --- Event Listener ---
        inputFile.off('change.fileNameDisplay').on('change.fileNameDisplay', function (e) {
            // Added .fileNameDisplay to namespace the event, preventing potential multiple bindings
            let fileName = '';
            if (e.target.files && e.target.files.length > 0) {
                fileName = e.target.files[0].name;
                console.log("File selected for input ID:", inputId, "-", fileName); // Log: File selected
            } else {
                console.log("File selection cleared for input ID:", inputId); // Log: Selection cleared
            }
            displaySpan.text(fileName); // Update the span
        });

        console.log("Attached change listener for input ID:", inputId); // Log: Listener attached
    });
    console.log("Finished setting up custom file inputs."); // Log: Function finished
}

// --- Document Ready ---
$(document).ready(function () {
    console.log("Document ready."); // Log: Document is ready

    // Initialize DataTables if needed
    if ($.fn.DataTable) {
        try {
            $('.datatable').DataTable();
            console.log("DataTables initialized."); // Log: DataTables success
        } catch (e) {
            console.error("Error initializing DataTables:", e); // Log: DataTables error
        }
    } else {
        console.warn("DataTables plugin not found."); // Log: DataTables missing
    }

    // Call the setup function for file inputs
    setupCustomFileInputs();
});