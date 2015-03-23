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
                    return '<a href="#" onclick="copyToClipBoardhtml(\'' + value + '\')">复制链接</a>' +
                    '<a href="javascript:void(0);" onclick="SendMailInit(1,\'' + value + '\')"><img src="/Content/img/mail.gif" alt="发送邮件" title="发送邮件" width="16" height="16" /></a>';
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
                //$.messager.alert("系统提示", "您选择的是文件，不是文件夹！", "warning");
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
    

    //新建文件夹
    function AddFlodDialog() {
        $('#dialogview').show();
        $('#dialogview').dialog({
            title:'新建目录',
            buttons: [{
                text: '提交',
                iconCls: 'icon-ok',
                handler: function () {
                    $("#errormsg").html("");
                    if ($("#newFold").val() == "") {
                        $("#errormsg").html("请填写新文件夹名称");
                        return false;
                    }
                    var newFold = $("#newFold").val();
                    var _currentPath = $("#smc a:last").text() + "/" + newFold;
                    $.ajax({
                        url: "/FileStore/AddFold",
                        type: 'POST',
                        cache: false,
                        async: false,
                        data: { "NewFoldName": _currentPath },
                        error: function (request) {
                            alert(request);
                        },
                        success: function (data) {
                            $('#dialogviewMsg').show();
                            $('#dialogviewMsg').dialog({
                                buttons: [{
                                    text: '取消',
                                    handler: function () {
                                        $('#dialogviewMsg').dialog('close');
                                    }
                                }]
                            });
                            if (data == "success") {
                                $("#messageShow").html("恭喜！创建新文件夹成功");
                                //window.location.reload();
                                $('#fileList').datagrid({
                                    url: '/FileStore/AjaxJson?localpath=' + _currentPath
                                })
                                addFileMapPath(_currentPath)
                            }
                            else if (data == "false") {
                                $("#messageShow").html("抱歉！创建新文件夹失败");
                            }
                        }
                    });
                }
            }, {
                text: '取消',
                handler: function () {
                    $('#dialogview').dialog('close');
                }
            }]
        });
    }

 
 //上传文件
    function UpdloadFiles() {
        $('#UploadFilesDialog').show();
        $('#UploadFilesDialog').dialog({
            buttons: [{
                text: '上传',
                iconCls: 'icon-ok',
                handler: function () {
                    if (CheckUpImg()) {
                        var curpath = $("#smc a:last").text();
                        $('#uploadForm').ajaxSubmit({
                            url: '/FileStore/UploadFile?curpath=' + curpath,
                            resetForm: false,
                            dataType: "json",
                            success: function (data) {
                                var jsonObj = eval('(' + data + ')');
                                if (jsonObj.result == "ok") {
                                    $.messager.alert("系统提示", "恭喜！上传文件成功");
                                }
                                else if (jsonObj.result == "error") {
                                    $.messager.alert("系统提示", "抱歉！上传文件失败");
                                }
                            }
                        });
                    }
                    else {
                        return false;
                    }
                }
            }, {
                text: '取消',
                handler: function () {
                    $('#UploadFilesDialog').dialog('close');
                }
            }]
        });
    }

    //设置多文件上传
    function MoreFile() {
        var _count = $("#filesCount").val();
        writeHtml(_count);
    }

    function writeHtml(count) {
        var _input = "";
        for (var i = 0; i < count; i++) {
            _input = _input + "<li><input type=\"file\" id=\"file" + i + "\" name=\"uploadFileTool\"/><span id=\"f" + i + "\" style=\"font-size:12px; color:Red\"></span></li>";
        }
        $("#moreUptool").html(_input);
    }

    //检查上传文件
    function CheckUpImg() {
        var els = $("#moreUptool input");
        var flag = true;
        for (var i = 0; i < els.length; i++) {
            if (els[i].type == "file") {
                if (els[i].value == "") {
                    $("#f" + i).html("选择要上传的文件");
                    return false;
                }
                else {
                    $("#f" + i).html("");
                    var imgUrl = els[i].value;
                    var ename = imgUrl.substring(imgUrl.lastIndexOf('.') + 1, imgUrl.lastIndexOf('.') + 4);
                    if (!(ename.toLowerCase() == "jpg" || ename.toLowerCase() == "bmp" ||
                         ename.toLowerCase() == "rar" || ename.toLowerCase() == "pdf" || ename.toLowerCase() == "ppt" || ename.toLowerCase() == "pptx" ||
                         ename.toLowerCase() == "xls" || ename.toLowerCase() == "xlsx" || ename.toLowerCase() == "zip" ||
                         ename.toLowerCase() == "png" || ename.toLowerCase() == "gif" || ename.toLowerCase() == "wmv" || ename.toLowerCase == "rmvb" || ename.toLocalCase == "rm" ||
                         ename.toLowerCase() == "flv" || ename.toLowerCase() == "js" || ename.toLowerCase() == "css" || ename.toLowerCase() == "htm" || ename.toLowerCase() == "html"
                         )) {
                        $("#f" + i).html("文件格式不正确");
                        return false;
                    } else {
                        $("#f" + i).html("");
                        var img = new Image();
                        img.src = imgUrl;
                        if ((img.fileSize / 1024) > 500) {
                            $("#f" + i).html("文件太大");
                            return false;
                        }
                        $("#f" + i).html("");
                    }
                }
            }
        }
        return true;
    }

    function SendMailInit(ty, url) {
                SendMail(ty, url);
    }

    function SendMail(ty, url) {
        $('#MailDialog').show();
        $('#MailDialog').dialog({
            title: '发信',
            buttons: [{
                text: '提交',
                iconCls: 'icon-ok',
                handler: function () {
                    var mailTo = $("#receive").val();
                    var title = $("#title").val();
                    var content = $("#MailContent").val();
                    $.ajax({
                        url: "/FileStore/SendMail",
                        type: 'POST',
                        cache: false,
                        async: false,
                        data: { fileUrl: url, mailTo: mailTo, title: title, content: content },
                        error: function (xhr) {
                            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
                        },
                        success: function (data) {
                            if (data == "success") {
                                $.messager.alert("系统提示", "发信成功");
                                //PageInit();
                            }
                            else if (data == "error") {
                                $.messager.alert("系统提示", "发信失败,请检查您的密码和收信人地址是否有误,", "warning");
                            }
                            else {
                                $.messager.alert(data);
                            }
                        }
                    });
                }
            }, {
                text: '取消',
                handler: function () {
                    $('#MailDialog').dialog('close');
                }
            }]
        });
    }
    function PageInit() {
        window.location.reload();
    }
