CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    config.language = 'zh-cn';
    config.htmlEncodeOutput = true;
    config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html',
    config.filebrowserImageBrowseUrl = "/ImgUpload/Index/";
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    config.startupMode = 'source';
    config.autoUpdateElement = false;
    config.entities_greek = false;
    config.entities_latin = false;
    config.forcePasteAsPlainText = false;
    config.entities_processNumerical = false;
    config.forceSimpleAmpersand = false;
    config.ignoreEmptyParagraph = false;
    config.skin = 'v2';

    //工具栏
    config.toolbar =
    [
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link', 'Unlink', 'Anchor'],
        ['Image', 'file', 'Table', 'SpecialChar', 'Preview'],
        '/',
        ['Styles', 'Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Maximize', 'ShowBlocks', '-', 'Source', '-', 'Undo', 'Redo']
    ];
    config.extraPlugins = "file";
};
