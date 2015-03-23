function UploadImgDialog(objID, valueControlID) {
    $("#" + objID).click(function () {
        //打开模态子窗体,并获取返回值
        var result = showModalDialog('/ImgUpload/?Type=Custom', '上传图片', 'dialogWidth:770px;dialogHeight:510px;center:yes;help:no;resizable:no;status:no');
        $("#" + valueControlID).val(result);
    })
}