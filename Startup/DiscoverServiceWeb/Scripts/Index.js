var typeCache = new Array();

function OpenInterfaceDetail(interfaceName) {
    var url = "/ServiceDiscovery/ServiceDetail/" + interfaceName;
    window.open(url, "_Blank");
}

function ClickImg(img) {
    var fullTypeName = $(img).next().val();
    var isExpand = true;
    var type = null;
    if ($(img).attr("src").indexOf("icon-right.png") > -1) {
        isExpand = false;
    }
    if (isExpand) {
        $(img).parent().children("ul").remove();
        $(img).attr("src", "/Images/Jqx/icon-right.png");
    }
    else {
        for (var i = 0; i < typeCache.length; i++) {
            if (typeCache[i].FullTypeName == fullTypeName) {
                type = typeCache[i];
                break;
            }
        }
        if (type == null) {
            $(img).attr("src", "/Images/Jqx/loader.gif");
            $.ajax({
                url: "/ServiceDiscovery/GetTypeDescription/" + fullTypeName.replace(/\./g, "-"),
                success: function (data) {
                    AppendType(data, $(img).parent());
                    typeCache.push(data);
                    if (isExpand) {
                        $(img).attr("src", "/Images/Jqx/icon-right.png");
                    }
                    else {
                        $(img).attr("src", "/Images/Jqx/icon-down.png");
                    }
                }
            });
        }
        else {
            AppendType(type, $(img).parent());
            $(img).attr("src", "/Images/Jqx/icon-down.png");
        }
    }
}

function AppendType(type, li) {
    var html = "";
    if (type.Properties != null && type.Properties.length > 0) {

        html += "<ul id=\"treeType\" class=\"list-unstyled\" style=\"margin-left:20px;\">";
        for (var i = 0; i < type.Properties.length; i++) {
            var property = type.Properties[i];
            html += "<li>";
            if (property.PropertyTypeInfo.IsClass) {
                html += "<img src=\"/Images/Jqx/icon-right.png\" style=\"cursor:pointer;width:17px;height:17px;\" onclick=\"ClickImg(this);\" />";
                html += "<input type=\"hidden\" value=\"" + property.PropertyTypeInfo.FullTypeName + "\" />";
                html += "<span>" + property.PropertyName + "</span>";
            }
            else {
                html += "<span style=\"padding-left:17px;\">" + property.PropertyName + "</span>";
            }
            html += " <span>--</span> ";
            if (property.PropertyTypeInfo.IsClass) {
                html += "<a href=\"/ServiceDiscovery/TypeDescription/" + property.PropertyTypeInfo.FullTypeName.replace(/\./g, "-") + " target=\"_blank\"\">";
                html += property.PropertyTypeInfo.TypeName;
                html += "</a>";
            }
            else {
                html += "<span>" + property.PropertyTypeInfo.TypeName + "</span>"
            }
            html += "</li>"
        }
        html += "</ul>";
    }
    $(li).append(html);
}