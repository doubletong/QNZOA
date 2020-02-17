
var RichTextEditor = {
    removeTinymce: function () {
        tinymce.remove();
    },
    loadTinymce: function (mytextarea) {
        tinymce.init({
            selector: '#' + mytextarea
        });
    },
    loadFullTinymce: function (mytextarea) {     

        tinymce.init({
            selector: '#' + mytextarea,
            //picture manager
            file_picker_callback: QNZ.FilePickerCallback,   //from plugin FileManager.js
            images_upload_handler: QNZ.ImagesUploadHandler,
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
        
            templates: [
                { title: 'New Table', description: 'creates a new table', content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
                { title: 'Starting my story', description: 'A cure for writers block', content: 'Once upon a time...' },
                { title: 'New list with dates', description: 'New List with dates', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
            ],
            template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
            template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
       
            image_caption: true,
            quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
            noneditable_noneditable_class: "mceNonEditable",
            toolbar_drawer: 'sliding',
            contextmenu: "link image imagetools table",
            //setup: function (ed) {
            //    ed.on('change', function (e) {
               
            //        var content = tinyMCE.get(ed.id).getContent();
            //        console.log(content);
            //        var escapedClassName = ed.id.replace(/(\[|\])/g, '\\$&');
                    
            //      //  $('#' + escapedClassName).html(content).tirgger("change");
            //        var d = document.getElementById(escapedClassName);
            //        console.log(d);
            //        d.innerHTML = content;                 
            //        d.dispatchEvent(new Event('change'));   // 解决动态改变值不触发change事件的问题
            //    });
            //}
        });
    },
    getTinymceContent:function(el) {
        return tinyMCE.get(el).getContent()
    }

};


var App = {

    holder: function (myImage){
        var myImage = document.getElementById(myImage);
        Holder.run({
            images: myImage
        });       
    },

    CallSingleFinder: function () {
        QNZ.selectActionFunction = SetThumbnail;
        QNZ.open();
    },

    SiteInit: function () {
        // 菜单折叠
        $("#mainmenu .down-nav>a").click(function (e) {
            e.preventDefault();
            var $that = $(this);
            $that.next('.submenu').slideToggle(function () {
                $that.closest('li.down-nav').toggleClass('open')
            });
        })
    }
}



function SetThumbnail(fileUrl) {
    //$('#imgLogo').attr("src", fileUrl);   
    document.getElementById("imgLogo").src = fileUrl;
    var d = document.getElementById("inputLogo");
  
    d.value = fileUrl;  
    d.dispatchEvent(new Event('change'));   // 解决动态改变值不触发change事件的问题
   
}

