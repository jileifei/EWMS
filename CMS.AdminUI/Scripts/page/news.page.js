jQuery(document).ready(function () {

    var DatePickerOpts = {
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy-mm-dd'
    };
    $('#txtBeginTime').datepicker(DatePickerOpts);
    $('#txtEndTime').datepicker(DatePickerOpts);

    $('#txtChannelName').combotree({
        url: '/channel/BrowseAuthAjaxLoading/',
        onSelect: function (node) {
            $("#txtChannelID").val(node.id);
        }
    });

    $("#btnReset").click(function () {
        $("#txtChannelID").val('');
        $('#txtChannelName').combotree("clear");
        $('#txtBeginTime').val('');
        $('#txtEndTime').val('');
        $("#txtID").val('');
        $("#txtTitle").val('');
    });
    // init page
    PageInit(0);
    // 搜索
    $("#btnSearch").click(function () {
        PageInit(0);
    });
});

function PageInit(page) {
    var template = '<tr><td class="a-center">${ID}</td><td><a href="${Linkurl}" target="_blank"><font color="${TitleColor}">${GenTitle(Title)}</font></a></td><td>${Source}</td><td>${ChannelName}</td><td>${Author}</td><td>${GetDate(PublicTime)}</td><td align="center"><span id="spanAuditing${ID}" style="cursor:pointer;" onclick="AuditingNews(${ID})">${AuditingStats}</span></td><td align="center"><a href="javascript:void(0);" onclick="EditNews(${ID});"><img src="/Content/img/edit.png" alt="修改文章" title="修改文章" width="16" height="16" /></a><a href="javascript:void(0);" onclick="DelNews(${ID});"><img src="/Content/img/cancel.png" alt="删除文章" title="删除文章" width="16" height="16" /></a></td></tr>';
    /* Compile the template as a named template */
    $.template("TableTemplate", template);

    $.ajax({
        type: "post",
        dataType: "json",
        url: "/news/serverajax", //请求的url
        data: { PageIndex: parseInt(page + 1), PageSize: 15, Title: $("#txtTitle").val(), ChannelID: $("#txtChannelID").val(), ID: $("#txtID").val(), BeginTime: $("#txtBeginTime").val(), EndTime: $("#txtEndTime").val() },
        success: function (data) {
            $("#divPagination").empty();
            //分页事件
            $("#divPagination").pagination(data.TotalRecords, {
                prev_text: "上一页",
                next_text: "下一页",
                num_edge_entries: 2,
                num_display_entries: 5,
                items_per_page: 15,
                //回调
                callback: PageCallBack
            });
        }
    });
}

function PageCallBack(page) {
    $("#tbodyContent").empty();
    
    $.ajax({
        type: "post",
        dataType: "json",
        url: "/news/serverajax", //请求的url
        data: { PageIndex: parseInt(page + 1), PageSize:15, Title: $("#txtTitle").val(), ChannelID: $("#txtChannelID").val(), ID: $("#txtID").val(), BeginTime: $("#txtBeginTime").val(), EndTime: $("#txtEndTime").val() },
        success: function (data) {
            $.tmpl("TableTemplate", data.ItemList).appendTo("#tbodyContent");
        }
    });
}

function GenTitle(title) {
    return $('<div/>').html(title).text();
}

function GetDate(jsondate) {
    if (jsondate != null && jsondate != "") {
        var date = new Date(parseInt(jsondate.replace("/Date(", "").replace(")/", "")));
        return dateFormat(date, "yyyy-mm-dd HH:MM:ss");
    }
    else {
        return jsondate;
    }
}

function EditNews(newsid) {
    var isHaveRight = CheckIsHaveRight(newsid, 1);
    if (!isHaveRight) {
        alert('对不起，您没有该频道下文章的编辑权限');
        return false;
    }
    window.open("/news/edit/?NewsID=" + newsid);
}

function DelNews(newsid) {
    var isHaveRight = CheckIsHaveRight(newsid, 2);
    if (!isHaveRight) {
        alert('对不起，您没有该频道下文章的删除权限');
        return false;
    }
    if (confirm('您确认要删除当前文章吗？')) {
        $.ajax({
            type: 'POST',
            url: '/news/deletenews/',
            data: { NewsID: newsid },
            cache: false,
            success: function (data) {
                data = eval('(' + data + ')');
                if (data.result == "ok") {
                    window.location.reload();
                }
                else {
                    alert(data.msg);
                }
            },
            error: function (xhr) {
                throw new Error('数据源访问错误' + '\n' + xhr.responseText);
            }
        })
    }
}

function AuditingNews(newsid) {
    var isHaveRight = CheckIsHaveRight(newsid, 3);
    if (!isHaveRight) {
        alert('对不起，您没有该频道下文章的审核权限');
        return false;
    }
    var type = $('#spanAuditing' + newsid).text();
    var auditing = 0;
    if (type == "未审核") {
        auditing = 1;
    }
    else {
        auditing = 0;
    }
    var msg = "";
    if (auditing == 1) {
        msg = "您确认要审核通过吗？";
    }
    else {
        msg = "您确认要取消审核吗？";
    }
    if (confirm(msg)) {
        $.ajax({
            type: 'POST',
            url: '/news/auditingnews/',
            data: { NewsID: newsid, Auditing: auditing },
            cache: false,
            success: function (data) {
                data = eval('(' + data + ')');
                if (data.result == "ok") {
                    if (auditing == 1) {
                        alert("审核成功");
                        $('#spanAuditing' + newsid).text("已审核");
                    }
                    else {
                        alert("取消审核成功");
                        $('#spanAuditing' + newsid).text("未审核");
                    }
                }
                else {
                    alert(data.msg);
                }
            },
            error: function (xhr) {
                throw new Error('数据源访问错误' + '\n' + xhr.responseText);
            }
        })
    }
}

function CheckIsHaveRight(NewsID, OperType) {
    var isHaveRight = false;
    $.ajax({
        type: 'POST',
        url: '/news/CheckIsHaveRight/',
        data: { NewsID: NewsID, checkType: OperType },
        cache: false,
        async: false,
        success: function (data) {
            if (data) {
                isHaveRight = true;
            }
        },
        error: function (xhr) {
            throw new Error('数据源访问错误' + '\n' + xhr.responseText);
        }
    });
    return isHaveRight;
}