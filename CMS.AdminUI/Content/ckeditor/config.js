CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
    config.language = 'zh-cn';
    config.htmlEncodeOutput = true;
    config.filebrowserBrowseUrl= '/ckfinder/ckfinder.html',
    config.filebrowserImageBrowseUrl = "/ImgUpload/Index/";
    // config.uiColor = '#AADC6E';
    config.skin = 'v2';
    //工具栏
    config.toolbar =
    [
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link',  'Unlink', 'Anchor'],
        ['Image','Flash',, 'file', 'Table', 'SpecialChar'],
        '/',
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', '-', 'Undo', 'Redo', '-', 'Source', 'Preview']
    ];
    config.extraPlugins="file";
};
