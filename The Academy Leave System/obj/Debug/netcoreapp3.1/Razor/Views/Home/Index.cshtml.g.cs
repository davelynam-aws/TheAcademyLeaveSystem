#pragma checksum "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7fc8e8a7f850a699a0a8279768894a59891f60c9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\_ViewImports.cshtml"
using The_Academy_Leave_System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\_ViewImports.cshtml"
using The_Academy_Leave_System.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7fc8e8a7f850a699a0a8279768894a59891f60c9", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5b058dc087dbb1f6af18ba5784b076f527ef8eb8", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<The_Academy_Leave_System.ViewModels.UserViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <h1 class=\"display-4\">");
#nullable restore
#line 8 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                     Write(CurrentUser.FirstName);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 8 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                                            Write(CurrentUser.LastName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n    <hr />\r\n\r\n\r\n\r\n    <h3>This Year ");
#nullable restore
#line 13 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
             Write(DateTime.Now.Year);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n    <h4>Leave Allowance: ");
#nullable restore
#line 14 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                    Write(Model.ThisUser.LeaveAllowanceThisYear);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Days</h4>\r\n    <h4>Days Booked: ");
#nullable restore
#line 15 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                Write(Model.LeaveBookedThisYear);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    <h4>Days Awaiting Approval: ");
#nullable restore
#line 16 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                           Write(Model.LeaveAwaitingApprovalThisYear);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    <h4>Days Left: ");
#nullable restore
#line 17 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
              Write(Model.LeaveLeftThisYear);

#line default
#line hidden
#nullable disable
            WriteLiteral(".</h4>\r\n    <hr />\r\n    <h3>Next Year ");
#nullable restore
#line 19 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
             Write(DateTime.Now.AddYears(1).Year);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n    <h4>Leave Allowance: ");
#nullable restore
#line 20 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                    Write(Model.ThisUser.LeaveAllowanceNextYear);

#line default
#line hidden
#nullable disable
            WriteLiteral(" Days</h4>\r\n    <h4>Days Booked: ");
#nullable restore
#line 21 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                Write(Model.LeaveBookedNextYear);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    <h4>Days Awaiting Approval: ");
#nullable restore
#line 22 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
                           Write(Model.LeaveAwaitingApprovalNextYear);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n    <h4>Days Left: ");
#nullable restore
#line 23 "C:\Users\dlynam\source\repos\The Academy Leave System\The Academy Leave System\Views\Home\Index.cshtml"
              Write(Model.LeaveLeftNextYear);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n\r\n\r\n\r\n\r\n\r\n\r\n</div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<The_Academy_Leave_System.ViewModels.UserViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
