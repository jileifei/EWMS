function CommonPage() {
    this.title = "";
    this.url = "";
}
// 选择系统变量
CommonPage.prototype.SelSystemVar = function () {
    var $dialog = null;
    var page = '/Global/Var/';
    var pagetitle = "选择系统变量"
    if ($("#divSelSystemVar").length == 0) {
        
        $dialog = $('<div id="divSelSystemVar"></div>')
               
    }
    else {
        $dialog = $('#divSelSystemVar')
    }
    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
                .dialog({
                    autoOpen: false,
                    modal: true,
                    height: 250,
                    width: 630,
                    title: pagetitle
                });
        $dialog.dialog('open');
}

// 选择模板
// tType：模板类型 1=栏目模板 2=新闻模板 3=列表页模板 4=专题模板 5=其他模板，导航，数据块等
CommonPage.prototype.SelTemplate = function (tType) {
    var $dialog = null;
    var page = '/Template/SelTemplate/?Type=' + tType;
    var pagetitle = "选择模板"
    if ($("#divSelTemplateVar").length == 0) {
        $dialog = $('<div id="divSelTemplateVar"></div>');
    }
    else {
        $dialog = $('#divSelTemplateVar');
    }
    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
            .dialog({
                autoOpen: false,
                modal: true,
                height: 250,
                width: 630,
                title: pagetitle
            });
    $dialog.dialog('open');
}

// 选择数据块
CommonPage.prototype.SelDataBlock = function () {
    var $dialog = null;
    var page = '/DataBlock/SelDataBlock/';
    var pagetitle = "选择数据块"
    if ($("#divDataBlockVar").length == 0) {
        $dialog = $('<div id="divDataBlockVar"></div>');
    }
    else {
        $dialog = $('#divDataBlockVar');
    }
    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
            .dialog({
                autoOpen: false,
                modal: true,
                height: 300,
                width: 630,
                title: pagetitle
            });
    $dialog.dialog('open');
}

// 选择全局变量
CommonPage.prototype.SelGlobalVar = function () {
    var $dialog = null;
    var page = '/Global/SelGlobal/';
    var pagetitle = "选择全局变量"
    if ($("#divGlobalVar").length == 0) {
        $dialog = $('<div id="divGlobalVar"></div>');
    }
    else {
        $dialog = $('#divGlobalVar');
    }

    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
            .dialog({
                autoOpen: false,
                modal: true,
                height: 300,
                width: 630,
                title: pagetitle
            });
    $dialog.dialog('open');
}

// 选择系统用户
CommonPage.prototype.SelSystemUser = function () {
    var $dialog = null;
    var page = '/User/SelUser/';
    var pagetitle = "选择系统用户"
    if ($("#divSystemUser").length == 0) {
        $dialog = $('<div id="divSystemUser"></div>');
    }
    else {
        $dialog = $('#divSystemUser');
    }

    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
            .dialog({
                autoOpen: false,
                modal: true,
                height: 300,
                width: 630,
                title: pagetitle
            });
    $dialog.dialog('open');
}

// 选择分页配置
CommonPage.prototype.SelPagerSet = function () {
    var $dialog = null;
    var page = '/PagerSet/SelPagerSet/';
    var pagetitle = "选择分页配置"
    if ($("#divPagerSet").length == 0) {
        $dialog = $('<div id="divPagerSet"></div>');
    }
    else {
        $dialog = $('#divPagerSet');
    }

    $dialog.html('<iframe style="border:0px;" src="' + page + '" width="100%" height="100%"></iframe>')
            .dialog({
                autoOpen: false,
                modal: true,
                height: 300,
                width: 630,
                title: pagetitle
            });
    $dialog.dialog('open');
}

// 文本框插入扩展
jQuery.fn.extend({
    insertAtCaret: function (myValue) {
        return this.each(function (i) {
            if (document.selection) {
                //For browsers like Internet Explorer
                this.focus();
                sel = document.selection.createRange();
                sel.text = myValue;
                this.focus();
            }
            else if (this.selectionStart || this.selectionStart == '0') {
                //For browsers like Firefox and Webkit based
                var startPos = this.selectionStart;
                var endPos = this.selectionEnd;
                var scrollTop = this.scrollTop;
                this.value = this.value.substring(0, startPos) + myValue + this.value.substring(endPos, this.value.length);
                this.focus();
                this.selectionStart = startPos + myValue.length;
                this.selectionEnd = startPos + myValue.length;
                this.scrollTop = scrollTop;
            } else {
                this.value += myValue;
                this.focus();
            }
        })
    }
});

Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) { return false; }
    this.splice(dx, 1);
} 