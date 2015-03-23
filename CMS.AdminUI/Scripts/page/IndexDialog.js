//文件列表
var CurrentFile;
var CurrentExtName;
$(function () {
    $('#fileList').datagrid({
        nowrap: true,
        striped: true,
        sortName: 'Filesize',
        sortOrder: 'desc',
        remoteSort: false,
        url: '/FileStore/AjaxJson',
        frozenColumns: [[
	                { field: 'ck', checkbox: true }
				]],
        columns: [[
	            { field: 'Filename', title: '名称', width: 200, sortable: true },
                { field: 'Filesize', title: '大小(KB)', width: 100, sortable: true },
                { field: 'ExtName', title: '类型', width: 50, sortable: true },
                { field: 'UploadDate', title: '创建时间', width: 150, sortable: true, formatter: function (value, rec) {
                    var ct = renderTime(value);
                    return ct;
                }
                },
                { field: 'FileURL', title: '链接地址', width: 80, sortable: true, formatter: function (value, rec) {
                    return '<a href="#" onclick="copyToClipBoardhtml(\'' + value + '\')">复制链接</a>';
                }
                }
				]],
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            CurrentFile = rowData.Filename;
            CurrentExtName = rowData.ExtName;
            $('#menuRight').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (rowData.FileType == 0) {
                $('#fileList').datagrid({
                    url: '/FileStore/AjaxJson?localPath=' + rowData.LocalPath
                });
                addFileMapPath(rowData.LocalPath);
            } else {
                $.messager.alert("系统提示", "您选择的是文件，不是文件夹！", "warning");
            }
        },
        onClickRow: function (rowIndex, rowData) {
            CurrentFile = rowData.Filename;
            CurrentExtName = rowData.ExtName;
        },
        pagination: true,
        rownumbers: true
    });
    var p = $('#fileList').datagrid('getPager');
    if (p) {
        $(p).pagination({
            pageSize: 10,
            onBeforeRefresh: function () {
            }
        });
    }
});

function okAdd() {
    if (CurrentExtName == "") {
        $.messager.alert("系统提示", "您选择的不是文件，是文件夹！", "warning");
        return;
    }
    if (CurrentExtName == ".jpg" || CurrentExtName == ".png" || CurrentExtName == ".bmp")
    {
        window.returnValue = "<img src=\"" + CurrentFile + "\">";
    }
    if (CurrentExtName == ".ppt" || CurrentExtName == ".pdf" || CurrentExtName == ".xls" ||
    CurrentExtName == ".xlsx" || CurrentExtName == ".doc" || CurrentExtName == ".docx" ||
    CurrentExtName == ".pptx" || CurrentExtName == ".zip" || CurrentExtName == ".rar") {
        window.returnValue = "<p><a href=\"" + CurrentFile + "\" target=\"_blank\">下载附件</a></p>";
    }

    if (CurrentExtName == ".swf" || CurrentExtName == ".flv") {
        var str = "<object width=\"150\" height=\"150\" codebase=\"http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0\" classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" id=\"flashgame\">";
        str += "<param value=\"" + CurrentFile + "\" name=\"movie\">";
        str += "<param value=\"high\" name=\"quality\">";
        str += "<embed width=\"150\" height=\"150\" wmode=\"transparent\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" quality=\"high\" src=\"" + CurrentFile + "\">";
        str += "</object>";
        window.returnValue = str;
    }   
    window.close();
}

function copyToClipBoardhtml(obj) {
    var clipBoardContent = obj;
    window.clipboardData.setData("Text", clipBoardContent);
    alert("地址复制成功");
}
//文件夹路径
function fileMapControl(id) {
    var p = "";
    p = $("#sm" + id).text();
    for (var i = 1; i < 10; i++) {
        var index = parseInt(id) + i;
        $("#sm" + index).remove();
    }
    if (p == "根目录") {
        p = "";
    }
    $('#fileList').datagrid({
        url: '/FileStore/AjaxJson?localPath=' + p
    });
}

function addFileMapPath(path) {
    var index = $("#smc a:last").attr("id").replace("sm", "");
    index = parseInt(index) + 1;
    var smstr = "<a href=\"#\" class=\"easyui-linkbutton l-btn l-btn-plain\" plain=\"true\" onclick=\"javascript:fileMapControl(" + index + ");\" id=\"sm" + index + "\" >" + path + "</a>";
    $("#smc").append(smstr);
}

function renderTime(data) {
    var da = eval('new ' + data.replace('/', '', 'g').replace('/', '', 'g'));
    return da.getFullYear() + "/" + (da.getMonth() + 1) + "/" + da.getDate() + "/" + da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds();
}

function PageInit() {
    window.location.reload();
}
