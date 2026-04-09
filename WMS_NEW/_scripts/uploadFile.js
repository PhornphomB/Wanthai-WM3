function bindUpload(elmId, urlUpload, folderId, triggerId, _extensions) {
    $("#" + elmId).pluploadQueue({
        // General settings
        runtimes: 'html5,html4',
        url: urlUpload,

        multipart_params: {
            folder_id: folderId
        },

        // User can upload no more then 10 files in one go (sets multiple_queues to false)
        max_file_count: 1,
        //multiple_queues : false,

        chunk_size: '1mb',


        // Resize images on clientside if we can
        resize: {
            width: 600,
            height: 600,
            quality: 90,
            crop: true // crop to exact dimensions
        },

        filters: {
            // Maximum file size
            max_file_size: '10mb',
            // Specify what files to browse for
            mime_types: [
                {
                    title: "Custom files", extensions: _extensions
                }
            ]
        },

        // Rename files by clicking on their titles
        rename: false,

        // Sort files
        sortable: true,

        // Enable ability to drag'n'drop files onto the widget (currently only HTML5 supports that)
        dragdrop: true,


        // Post init events, bound after the internal events
        init: {
            UploadComplete: function (up, files) {

                up.splice(); //remove items of container
                up.refresh(); //refresh container

                $("#" + triggerId).click();
            }
        }
    });
}

function bindUploadFile(elmId, urlUpload, folderId, triggerId, _extensions) {
    $("#" + elmId).pluploadQueue({
        // General settings
        runtimes: 'html5,html4',
        url: urlUpload,

        multipart_params: {
            folder_id: folderId
        },

        // User can upload no more then 10 files in one go (sets multiple_queues to false)
        max_file_count: 10,

        chunk_size: '100mb',


        filters: {
            // Maximum file size
            max_file_size: '500mb',
            // Specify what files to browse for
            mime_types: [
                {
                    title: "Custom files", extensions: _extensions
                }
            ]
        },

        // Rename files by clicking on their titles
        rename: false,

        // Sort files
        sortable: true,

        // Enable ability to drag'n'drop files onto the widget (currently only HTML5 supports that)
        dragdrop: true,


        // Post init events, bound after the internal events
        init: {
            UploadComplete: function (up, files) {

                up.splice(); //remove items of container
                up.refresh(); //refresh container

                $("#" + triggerId).click();
            }
        }
    });
}

