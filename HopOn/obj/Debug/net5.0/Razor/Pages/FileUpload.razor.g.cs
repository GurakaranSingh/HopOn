#pragma checksum "E:\HopOn\CLinet Main HopOn\HopOn\Pages\FileUpload.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "36bbf642794e1fb221ba14152ed8a2c41fae7166"
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
    public partial class FileUpload : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h3>File Upload</h3>\r\n");
            __builder.AddMarkupContent(1, "<label for=\"formFileLg\" class=\"form-label\">Large file input example</label>\r\n");
            __builder.AddMarkupContent(2, @"<div class=""row""><div class=""col-lg-9""><input multiple class=""form-control form-control-lg"" id=""fileToUpload"" type=""file""></div>
    <div class=""col-lg-3""><button class=""btn btn-outline-secondary"" onclick=""returnArrayAsync()"">
            Upload
        </button></div></div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
