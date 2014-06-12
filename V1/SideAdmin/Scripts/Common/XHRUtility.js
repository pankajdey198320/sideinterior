(function ($) {
    $.fn.uploadAjax = function (files, url, jSonData, onProgress, onSuccess, onError) {
        var formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            var file = files[i];
            formData.append("FileUpload", file);
        }


        if (jSonData != null && jSonData != undefined) {
            var keys = Object.keys(jSonData);
            for (var i = 0; i < keys.length; i++) {
                formData.append(keys[i], jSonData[keys[i]]);
            }
            //formData.append("data", jSonData);
        }


        $.ajax({
            type: "POST",
            url: url,
            data: formData,
            contentType: false,
            processData: false,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                //Upload progress
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        if (onProgress && typeof (onProgress) === "function") {
                            onProgress(evt);
                        }
                    }
                }, false);
                return xhr;
            },
            success: function (response) {
                if (onSuccess && typeof (onSuccess) === "function") {
                    onSuccess(response);
                }
            },
            error: function (error) {
                if (onError && typeof (onError) === "function") {
                    onError(error);
                }
            }
        });
    }

    $.fn.submitAjax = function (url, jSonData, onProgress, onSuccess, onError) {
        $.ajax({
            type: "POST",
            url: url,
            data: jSonData,
            contentType: "application/json",
            processData: false,
            xhr: function () {
                var xhr = new window.XMLHttpRequest();
                //Upload progress
                xhr.upload.addEventListener("progress", function (evt) {
                    if (evt.lengthComputable) {
                        if (onProgress && typeof (onProgress) === "function") {
                            onProgress(evt);
                        }
                    }
                }, false);
                return xhr;
            },
            success: function (response) {
                if (onSuccess && typeof (onSuccess) === "function") {
                    onSuccess(response);
                }
            },
            error: function (error) {
                if (onError && typeof (onError) === "function") {
                    onError(error);
                }
            }
        });
    }

}(jQuery));
