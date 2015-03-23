var detailview = $.extend({}, $.fn.datagrid.defaults.view, {
    addExpandColumn: function(target) {
        var opts = $.data(target, 'datagrid').options;
        var body1 = $(target).datagrid('getPanel').find('div.datagrid-view1');
        body1.find('tr[datagrid-row-index]').each(function() {
            var tr = $(this);
            var rowIndex = tr.attr('datagrid-row-index');
            var cc = [];
            cc.push('<td>');
            cc.push('<div style="text-align:center;width:25px">');
            cc.push('<div class="datagrid-row-expander datagrid-row-expand" row-index=' + rowIndex + ' style="cursor:pointer;height:14px;" />');
            cc.push('</div>');
            cc.push('</td>');
            if (tr.is(':empty')) {
                tr.html(cc.join(''));
            } else if (tr.children('td.datagrid-td-rownumber').length) {
                $(cc.join('')).insertAfter(tr.children('td.datagrid-td-rownumber'));
            } else {
                $(cc.join('')).insertBefore(tr.children('td:first'));
            }
            //			tr.children('td.datagrid-td-rownumber').attr('rowspan', 2);
        });
    },

    render: function(target, container, frozen) {
        var opts = $.data(target, 'datagrid').options;
        var rows = $.data(target, 'datagrid').data.rows;
        var fields = $(target).datagrid('getColumnFields', frozen);
        var table = [];
        for (var i = 0; i < rows.length; i++) {
            table.push('<table cellspacing="0" cellpadding="0" border="0"><tbody>');

            // get the class and style attributes for this row
            var cls = (i % 2 && opts.striped) ? 'class="datagrid-row-alt"' : '';
            var styleValue = opts.rowStyler ? opts.rowStyler.call(target, i, rows[i]) : '';
            var style = styleValue ? 'style="' + styleValue + '"' : '';

            table.push('<tr datagrid-row-index="' + i + '" ' + cls + ' ' + style + '>');
            table.push(this.renderRow.call(this, target, fields, frozen, i, rows[i]));
            table.push('</tr>');

            table.push('<tr style="display:none;">');
            if (frozen) {
                table.push('<td colspan=' + (fields.length + 2) + ' style="border-right:0">');
            } else {
                table.push('<td colspan=' + (fields.length) + '>');
            }
            table.push('<div class="datagrid-row-detail">');
            if (frozen) {
                table.push('&nbsp;');
            } else {
                table.push(opts.detailFormatter.call(target, i, rows[i]));
            }
            table.push('</div>');
            table.push('</td>');
            table.push('</tr>');

            table.push('</tbody></table>');
        }

        $(container).html(table.join(''));
    },

    onBeforeRender: function(target) {
        var opts = $.data(target, 'datagrid').options;
        var panel = $(target).datagrid('getPanel');
        var t = panel.find('div.datagrid-view1 div.datagrid-header table');
        if (t.find('div.datagrid-header-expander').length) {
            return;
        }
        var td = $('<td rowspan="' + opts.frozenColumns.length + '"><div class="datagrid-header-expander" style="width:25px;"></div></td>');
        if ($('tr', t).length == 0) {
            td.wrap('<tr></tr>').parent().appendTo($('tbody', t));
        } else if (opts.rownumbers) {
            td.insertAfter(t.find('td:has(div.datagrid-header-rownumber)'));
        } else {
            td.prependTo(t.find('tr:first'));
        }
    },

    onAfterRender: function(target) {
        var state = $.data(target, 'datagrid');
        var opts = state.options;
        var panel = $(target).datagrid('getPanel');
        var view = panel.find('div.datagrid-view');
        var view1 = view.children('div.datagrid-view1');
        var view2 = view.children('div.datagrid-view2');

        $.fn.datagrid.defaults.view.onAfterRender.call(this, target);

        if (!state.onResizeColumn) {
            state.onResizeColumn = opts.onResizeColumn;
        }
        if (!state.onResize) {
            state.onResize = opts.onResize;
        }
        function setBodyTableWidth() {
            var table = view2.find('div.datagrid-header table');
            var columnWidths = view2.find('div.datagrid-header table').width();
            view2.children('div.datagrid-body').children('table').width(columnWidths);
        }

        opts.onResizeColumn = function(field, width) {
            setBodyTableWidth();
            var rowCount = $(target).datagrid('getRows').length;
            for (var i = 0; i < rowCount; i++) {
                $(target).datagrid('fixDetailRowHeight', i);
            }

            // call the old event code
            state.onResizeColumn.call(target, field, width);
        };
        opts.onResize = function(width, height) {
            setBodyTableWidth();
            state.onResize.call(panel, width, height);
        };

        this.addExpandColumn(target);

        panel.find('div.datagrid-row-expander').unbind('.datagrid').bind('click.datagrid', function(e) {
            var rowIndex = $(this).attr('row-index');
            if ($(this).hasClass('datagrid-row-expand')) {
                $(this).removeClass('datagrid-row-expand').addClass('datagrid-row-collapse');
                $(target).datagrid('expandRow', rowIndex);
            } else {
                $(this).removeClass('datagrid-row-collapse').addClass('datagrid-row-expand');
                $(target).datagrid('collapseRow', rowIndex);
            }
            $(target).datagrid('fixRowHeight');
            return false;
        });
        view1.find('div.datagrid-footer div.datagrid-row-expander').css('visibility', 'hidden');
        $(target).datagrid('resize');
    }
});

$.extend($.fn.datagrid.methods, {
    fixDetailRowHeight: function(jq, index) {
        return jq.each(function() {
            var view = $(this).datagrid('getPanel').find('div.datagrid-view');
            var view1 = view.children('div.datagrid-view1');
            var view2 = view.children('div.datagrid-view2');
            var tr1 = view1.find('tr[datagrid-row-index=' + index + ']').next();
            var tr2 = view2.find('tr[datagrid-row-index=' + index + ']').next();
            // fix the detail row height
            if (tr2.is(':visible')) {
                tr1.css('height', '');
                tr2.css('height', '');
                var height = Math.max(tr1.height(), tr2.height());
                tr1.css('height', height);
                tr2.css('height', height);
            }
        });
    },
    expandRow: function(jq, index) {
        return jq.each(function() {
            var opts = $(this).datagrid('options');
            var view = $(this).datagrid('getPanel').find('div.datagrid-view');
            var view1 = view.children('div.datagrid-view1');
            var view2 = view.children('div.datagrid-view2');
            var tr1 = view1.find('tr[datagrid-row-index=' + index + ']').next();
            var tr2 = view2.find('tr[datagrid-row-index=' + index + ']').next();
            tr1.show();
            tr2.show();
            $(this).datagrid('fixDetailRowHeight', index);
            if (opts.onExpandRow) {
                var row = $(this).datagrid('getRows')[index];
                opts.onExpandRow.call(this, index, row);
            }
        });
    },
    collapseRow: function(jq, index) {
        return jq.each(function() {
            var opts = $(this).datagrid('options');
            var view = $(this).datagrid('getPanel').find('div.datagrid-view');
            var view1 = view.children('div.datagrid-view1');
            var view2 = view.children('div.datagrid-view2');
            var tr1 = view1.find('tr[datagrid-row-index=' + index + ']').next();
            var tr2 = view2.find('tr[datagrid-row-index=' + index + ']').next();
            tr1.hide();
            tr2.hide();
            if (opts.onCollapseRow) {
                var row = $(this).datagrid('getRows')[index];
                opts.onCollapseRow.call(this, index, row);
            }
        });
    }
});