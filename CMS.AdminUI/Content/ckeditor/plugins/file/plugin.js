(function () {
    //Section 1 : 按下自定义按钮时执行的代码
    var a = {
        exec: function (editor) {
            var ht = showModalDialog('/FileStore/IndexDialog/?CKEditor=fckContent', editor, 'dialogWidth:770px;dialogHeight:510px;center:yes;help:no;resizable:no;status:no');
            if (ht != null) {
                editor.insertHtml(ht);
            }

        }
    },
    //Section 2 : 创建自定义按钮、绑定方法
    b = 'file';
    CKEDITOR.plugins.add(b, {
        init: function (editor) {
            editor.addCommand(b, a);
            editor.ui.addButton('file', {
                label: '文件',
                icon: this.path + 'file.gif',
                command: b
            });
        }
    });
})();