#pragma checksum "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "99b6cb8aee4d1048f5cb199c9fe148f52a1a26c5"
// <auto-generated/>
#pragma warning disable 1591
namespace HopOn.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Components.Web.Virtualization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using HopOn;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using HopOn.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "E:\HopOn\CLinet Main HopOn\HopOn\_Imports.razor"
using Microsoft.AspNetCore.Components.ProtectedBrowserStorage;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor"
using HopOn.Services;

#line default
#line hidden
#nullable disable
    public partial class FileList : FileListBaseClass
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h3>File List</h3>\r\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "class", "card-deck");
#nullable restore
#line 5 "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor"
     if (FileLists != null)
    {
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor"
         foreach (var File in FileLists)
        {

#line default
#line hidden
#nullable disable
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "class", "card m-3");
            __builder.AddAttribute(5, "style", "min-width: 18rem; max-width:24.5%;");
            __builder.OpenElement(6, "div");
            __builder.AddAttribute(7, "class", "card-header");
            __builder.OpenElement(8, "h3");
            __builder.AddContent(9, 
#nullable restore
#line 11 "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor"
                         File.FileName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(10, "\r\n                <img class=\"card-img-top imageThumbnail\" src>\r\n                ");
            __builder.AddMarkupContent(11, "<div class=\"card-footer text-center\"><a href=\"#\" class=\"btn btn-primary m-1\">View</a>\r\n                    <a href=\"#\" class=\"btn btn-primary m-1\">Download</a>\r\n                    <a href=\"#\" class=\"btn btn-danger m-1\">Delete</a></div>");
            __builder.CloseElement();
#nullable restore
#line 20 "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor"
        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 20 "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileList.razor"
         
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
