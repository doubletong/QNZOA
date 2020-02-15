
var RichTextEditor = {
    
    loadTinymce: function (mytextarea) {
        tinymce.init({
            selector: '#' + mytextarea
        });
    },
    loadFullTinymce: function (mytextarea) {
        tinymce.init({
            selector: '#' + mytextarea,
            plugins: 'print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
            imagetools_cors_hosts: ['picsum.photos'],
            menubar: 'file edit view insert format tools table help',
            toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl',
            toolbar_sticky: true,
            autosave_ask_before_unload: true,
            autosave_interval: "30s",
            autosave_prefix: "{path}{query}-{id}-",
            autosave_restore_when_empty: false,
            autosave_retention: "2m",
            image_advtab: true,
            content_css: [
                '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
                '//www.tiny.cloud/css/codepen.min.css'
            ],
            link_list: [
                { title: 'My page 1', value: 'http://www.tinymce.com' },
                { title: 'My page 2', value: 'http://www.moxiecode.com' }
            ],
            image_list: [
                { title: 'My page 1', value: 'http://www.tinymce.com' },
                { title: 'My page 2', value: 'http://www.moxiecode.com' }
            ],
            image_class_list: [
                { title: 'None', value: '' },
                { title: 'Some class', value: 'class-name' }
            ],
            importcss_append: true,
            height: 400,
            file_picker_callback: function (callback, value, meta) {
                /* Provide file and text for the link dialog */
                if (meta.filetype === 'file') {
                    callback('https://www.google.com/logos/google.jpg', { text: 'My text' });
                }

                /* Provide image and alt text for the image dialog */
                if (meta.filetype === 'image') {
                    callback('https://www.google.com/logos/google.jpg', { alt: 'My alt text' });
                }

                /* Provide alternative source and posted for the media dialog */
                if (meta.filetype === 'media') {
                    callback('movie.mp4', { source2: 'alt.ogg', poster: 'https://www.google.com/logos/google.jpg' });
                }
            },
            templates: [
                { title: 'New Table', description: 'creates a new table', content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
                { title: 'Starting my story', description: 'A cure for writers block', content: 'Once upon a time...' },
                { title: 'New list with dates', description: 'New List with dates', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
            ],
            template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
            template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
            height: 600,
            image_caption: true,
            quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
            noneditable_noneditable_class: "mceNonEditable",
            toolbar_drawer: 'sliding',
            contextmenu: "link image imagetools table",
        });
    }

};



//var QNZ = {
//    ImagesUploadHandler: function (blobInfo, success, failure) {
//        var xhr, formData;

//        xhr = new XMLHttpRequest();
//        xhr.withCredentials = false;
//        xhr.open('POST', '/api/QNZFinder/ImageUpload');

//        xhr.onload = function () {
//            var json;

//            if (xhr.status != 200) {
//                failure('HTTP Error: ' + xhr.status);
//                return;
//            }

//            json = JSON.parse(xhr.responseText);

//            if (!json || typeof json.location != 'string') {
//                failure('Invalid JSON: ' + xhr.responseText);
//                return;
//            }

//            success(json.location);
//        };


//        var description = '';

//        jQuery(tinymce.activeEditor.dom.getRoot()).find('img').not('.loaded-before').each(
//            function () {
//                description = $(this).attr("alt");
//                $(this).addClass('loaded-before');
//            });

//        formData = new FormData();
//        formData.append('file', blobInfo.blob(), blobInfo.filename());
//        formData.append('description', description); //found now))

//        xhr.send(formData);
//    },


//    percent: 70,
//    baseUrl: "/QNZFinder/SingleFinder",
//    selectActionFunction: null,
//    elFinderCallback: function (fileUrl) {
//        this.selectActionFunction(fileUrl);
//    },
//    open: function () {
//        var w = 1140,
//            h = 600; // default sizes
//        if (window.screen) {
//            w = window.screen.width * this.percent / 100;
//            h = window.screen.height * this.percent / 100;
//        }
//        var x = screen.width / 2 - w / 2;
//        var y = screen.height / 2 - h / 2;

//        window.open(this.baseUrl, "_blank", 'height=' + h + ',width=' + w + ',left=' + x + ',top=' + y);
//    },

//    FilePickerCallback2: function (callback, value, meta) {
//        // Provide file and text for the link dialog
//        // if (meta.filetype == 'file') {
//        //   callback('mypage.html', {text: 'My text'});
//        // }

//        // // Provide image and alt text for the image dialog
//        //if (meta.filetype == 'image') {

//        //   callback('myimage.jpg', {alt: 'My alt text'});
//        // }

//        // // Provide alternative source and posted for the media dialog
//        // if (meta.filetype == 'media') {
//        //   callback('movie.mp4', {source2: 'alt.ogg', poster: 'image.jpg'});
//        // }
//        var finderUrl = '/QNZFinder/FinderForTinyMce';
//        tinyMCE.activeEditor.windowManager.openUrl({
//            url: finderUrl,
//            title: 'QNZFinder 1.0 文件管理',
//            width: 1140,
//            height: 700
//            // onMessage: function (api, data) {
//            //     if (data.mceAction === 'FileSelected') {
//            //        callback(data.url);
//            //        api.close();
//            //    }
//            //}
//        });

//        window.addEventListener('message', function (event) {
//            var data = event.data;
//            callback(data.content);
//        });

//    },

//    InitDropzone: function () {
//        Dropzone.options.dropzoneForm = {
//            paramName: "file",
//            maxFilesize: 20,
//            maxFiles: 5,
//            acceptedFiles: "image/*,application/pdf",
//            dictMaxFilesExceeded: "Custom max files msg",
//            dictDefaultMessage: "拖拽文件到这里上传",
//            queuecomplete: function (files) {
//                this.removeAllFiles();
//                console.log("queuecomplete");
//                document.getElementById("uploadbox").style.display = "none";

//                var filePath = document.getElementById("filePath");
//                var dir = filePath.innerText; //$(this).attr("data-dir"),
//                var url = "/qnzfinder/GetSubFiles?dir=" + dir;
//                SIG.getInstance().getFiles(url);
//            }
//        };
//    }

//};
