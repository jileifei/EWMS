jQuery(document).ready(function () {

    var DatePickerOpts = {
        changeMonth: true,
        changeYear: true,
        dateFormat: 'yy-mm-dd'
    };
    $('#txtBeginTime').datepicker(DatePickerOpts);
    $('#txtEndTime').datepicker(DatePickerOpts);

    $('#txtChannelName').combotree({
        url: '/channel/CombotreeAjaxLoading/',
        onSelect: function (node) {
            $("#txtChannelID").val(node.id);
        }
    });

    $("#btnReset").click(function () {
        SearchNewsID = 0;
        SearchChannelID = 0;
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
    var template = '<tr><td class="a-center">${ID}</td><td><a href="${Linkurl}" target="_blank"><font color="${TitleColor}">${GenTitle(Title)}</font></a></td><td>${Source}</td><td>${ChannelName}</td><td>${Author}</td><td>${GetDate(PublicTime)}</td><td align="center"><span id="spanAuditing${ID}" style="cursor:pointer;" onclick="AuditingNews(${ID})">${AuditingStats}</span></td><td align="center"><a href="javascript:void(0);" onclick="RestoreNews(${ID});"><img src="/Content/img/edit_add.png" alt="恢复菜品" title="恢复菜品" width="16" height="16" /></a></td></tr>';
    /* Compile the template as a named template */
    $.template("TableTemplate", template);

    $.ajax({
        type: "post",
        dataType: "json",
        url: "/news/RecycleServerAjax", //请求的url
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
        url: "/news/RecycleServerAjax", //请求的url
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


function RestoreNews(newsid) {
    if (confirm('您确认要恢复该菜品吗？')) {
        $.ajax({
            type: 'POST',
            url: '/news/RestoreNews/',
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