function TabHowverGroup(normalClass, howverClass, arrLabelIDS, arrDivIDS) {
    this.m_nclass = normalClass;
    this.m_hclass = howverClass;
    this.m_labids = arrLabelIDS;
    this.m_itemids = arrDivIDS;

    //wrap method to function
    this.funcwrap = function (ogrup, current) {
        return function () {
            for (var k = 0; k < ogrup.m_labids.length; k++) {
                var strmidk = "#" + ogrup.m_labids[k];
                if (ogrup.m_labids[k] == ogrup.m_labids[current]) {

                    if (ogrup.m_labids[k] == "lableScatter") {
                        $(strmidk).removeClass(ogrup.m_nclass).addClass("hover");
                    }
                    else {
                        $(strmidk).removeClass(ogrup.m_nclass).addClass(ogrup.m_hclass);
                    }

                } else {

                    if (ogrup.m_labids[k] == "lableScatter") {
                        $(strmidk).removeClass("hover").addClass("");
                    }
                    else {
                        $(strmidk).removeClass(ogrup.m_hclass).addClass(ogrup.m_nclass);
                    }

                }
            }


            for (var j = 0; j < ogrup.m_itemids.length; j++) {
                var strmid = "#" + ogrup.m_itemids[j];
                if (ogrup.m_itemids[current] == ogrup.m_itemids[j]) {
                    $(strmid).show();
                } else {
                    $(strmid).hide();
                }
            }
        };
    }

    if (this.m_labids.length != this.m_itemids.length) {
        alert("要切换的标签和内容ID数量不一至:标签IDS=" + this.m_labids);
        return;
    }
    //init 
    for (var i = 0; i < this.m_labids.length; i++) {
        var strid = this.m_labids[i];
        $("#" + strid).hover(this.funcwrap(this, i), function () { });
    }
}

//点击滑动门
function TabClickGroup(normalClass, howverClass, arrLabelIDS, arrDivIDS) {
    this.m_nclass = normalClass;
    this.m_hclass = howverClass;
    this.m_labids = arrLabelIDS;
    this.m_itemids = arrDivIDS;

    //wrap method to function
    this.funcwrap = function (ogrup, current) {
        return function () {
            for (var k = 0; k < ogrup.m_labids.length; k++) {
                var strmidk = "#" + ogrup.m_labids[k];
                if (ogrup.m_labids[k] == ogrup.m_labids[current]) {
                    $(strmidk).removeClass(ogrup.m_nclass).addClass(ogrup.m_hclass);
                } else {
                    $(strmidk).removeClass(ogrup.m_hclass).addClass(ogrup.m_nclass);
                }
            }


            for (var j = 0; j < ogrup.m_itemids.length; j++) {
                var strmid = "#" + ogrup.m_itemids[j];
                if (ogrup.m_itemids[current] == ogrup.m_itemids[j]) {
                    $(strmid).show();
                } else {
                    $(strmid).hide();
                }
            }
        };
    }


    if (this.m_labids.length != this.m_itemids.length) {
        alert("要切换的标签和内容ID数量不一至:标签IDS=" + this.m_labids);
        return;
    }

    //init 
    for (var i = 0; i < this.m_labids.length; i++) {
        var strid = this.m_labids[i];
        $("#" + strid).click(this.funcwrap(this, i));
    }
}