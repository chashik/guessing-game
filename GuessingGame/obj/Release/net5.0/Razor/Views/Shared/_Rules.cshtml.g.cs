#pragma checksum "E:\Alex\Repos\Assignments\GuessingGame\GuessingGame\Views\Shared\_Rules.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "627122763cd39cb2fbd1ca4905b5e9d9c5ae23ea"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Rules), @"mvc.1.0.view", @"/Views/Shared/_Rules.cshtml")]
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
#line 1 "E:\Alex\Repos\Assignments\GuessingGame\GuessingGame\Views\_ViewImports.cshtml"
using GuessingGame;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Alex\Repos\Assignments\GuessingGame\GuessingGame\Views\_ViewImports.cshtml"
using GuessingGame.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"627122763cd39cb2fbd1ca4905b5e9d9c5ae23ea", @"/Views/Shared/_Rules.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"093a5f78029041b8ac8073ce3aaec85b35d71f23", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Rules : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<h5 style=""text-align: center;"">Game rules:</h5>
<ul>
    <li>Program chooses a random secret number with 4 digits.</li>
    <li>All digits in the secret number are different.</li>
    <li>Player has 8 tries to guess the secret number.</li>
    <li>After each guess program displays the message ""M:m; P:p"" where: - m - number of matching digits but not on the right places - p - number of matching digits on exact places</li>
    <li>Game ends after 8 tries or if the correct number is guessed.</li>
</ul>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591