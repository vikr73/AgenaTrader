using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using AgenaTrader.API;
using AgenaTrader.Custom;
using AgenaTrader.Plugins;
using AgenaTrader.Helper;
using System.Windows.Forms;

namespace AgenaTrader.UserCode
{
    [Description("ScriptAcademy")]
    [Category("Script-Trading ScriptAcademy")]
    public class ScriptAcademy : UserIndicator
    {
        //Button
        private RectangleF _rect;

        //Form
        private Form _frm_lesson;

        //HTML
        private string myHTML = "";


        protected override void OnInit()
        {
        }

        protected override void OnStart()
        {

            // Add event listener
            if (Chart != null)
                Chart.ChartPanelMouseDown += OnChartPanelMouseDown;

        }

        protected override void OnCalculate()
        {

        }

        public string getResource(string FileName)
        {

            //todo dynamische HTML einlesen
            string myLessonHTML = "";
            if (FileName == "test_markdown_file_html_md")
            {
                myLessonHTML = " <!DOCTYPE html>\n <html>\n <head>\n <meta charset =\"utf-8\" />\n      <title>test_markdown_file</title>\n      <style>.markdown-preview:not([data-use-github-style]) { padding: 2em; font-size: 1.2em; color: rgb(171, 178, 191); overflow: auto; background-color: rgb(40, 44, 52); }\n.markdown-preview:not([data-use-github-style]) > :first-child { margin-top: 0px; }\n.markdown-preview:not([data-use-github-style]) h1, .markdown-preview:not([data-use-github-style]) h2, .markdown-preview:not([data-use-github-style]) h3, .markdown-preview:not([data-use-github-style]) h4, .markdown-preview:not([data-use-github-style]) h5, .markdown-preview:not([data-use-github-style]) h6 { line-height: 1.2; margin-top: 1.5em; margin-bottom: 0.5em; color: rgb(255, 255, 255); }\n.markdown-preview:not([data-use-github-style]) h1 { font-size: 2.4em; font-weight: 300; }\n.markdown-preview:not([data-use-github-style]) h2 { font-size: 1.8em; font-weight: 400; }\n.markdown-preview:not([data-use-github-style]) h3 { font-size: 1.5em; font-weight: 500; }\n.markdown-preview:not([data-use-github-style]) h4 { font-size: 1.2em; font-weight: 600; }\n.markdown-preview:not([data-use-github-style]) h5 { font-size: 1.1em; font-weight: 600; }\n.markdown-preview:not([data-use-github-style]) h6 { font-size: 1em; font-weight: 600; }\n.markdown-preview:not([data-use-github-style]) strong { color: rgb(255, 255, 255); }\n.markdown-preview:not([data-use-github-style]) del { color: rgb(124, 135, 156); }\n.markdown-preview:not([data-use-github-style]) a, .markdown-preview:not([data-use-github-style]) a code { color: rgb(82, 139, 255); }\n.markdown-preview:not([data-use-github-style]) img { max-width: 100%; }\n.markdown-preview:not([data-use-github-style]) > p { margin-top: 0px; margin-bottom: 1.5em; }\n.markdown-preview:not([data-use-github-style]) > ul, .markdown-preview:not([data-use-github-style]) > ol { margin-bottom: 1.5em; }\n.markdown-preview:not([data-use-github-style]) blockquote { margin: 1.5em 0px; font-size: inherit; color: rgb(124, 135, 156); border-color: rgb(75, 83, 98); border-width: 4px; }\n.markdown-preview:not([data-use-github-style]) hr { margin: 3em 0px; border-top: 2px dashed rgb(75, 83, 98); background: none; }\n.markdown-preview:not([data-use-github-style]) table { margin: 1.5em 0px; }\n.markdown-preview:not([data-use-github-style]) th { color: rgb(255, 255, 255); }\n.markdown-preview:not([data-use-github-style]) th, .markdown-preview:not([data-use-github-style]) td { padding: 0.66em 1em; border: 1px solid rgb(75, 83, 98); }\n.markdown-preview:not([data-use-github-style]) code { color: rgb(255, 255, 255); background-color: rgb(58, 63, 75); }\n.markdown-preview:not([data-use-github-style]) pre.editor-colors { margin: 1.5em 0px; padding: 1em; font-size: 0.92em; border-radius: 3px; background-color: rgb(49, 54, 63); }\n.markdown-preview:not([data-use-github-style]) kbd { color: rgb(255, 255, 255); border-width: 1px 1px 2px; border-style: solid; border-color: rgb(75, 83, 98) rgb(75, 83, 98) rgb(62, 68, 81); background-color: rgb(58, 63, 75); }\n.markdown-preview[data-use-github-style] { font-family: \"Helvetica Neue\", Helvetica, \"Segoe UI\", Arial, freesans, sans-serif; line-height: 1.6; word-wrap: break-word; padding: 30px; font-size: 16px; color: rgb(51, 51, 51); overflow: scroll; background-color: rgb(255, 255, 255); }\n.markdown-preview[data-use-github-style] > :first-child { margin-top: 0px !important; }\n.markdown-preview[data-use-github-style] > :last-child { margin-bottom: 0px !important; }\n.markdown-preview[data-use-github-style] a:not([href]) { color: inherit; text-decoration: none; }\n.markdown-preview[data-use-github-style] .absent { color: rgb(204, 0, 0); }\n.markdown-preview[data-use-github-style] .anchor { position: absolute; top: 0px; left: 0px; display: block; padding-right: 6px; padding-left: 30px; margin-left: -30px; }\n.markdown-preview[data-use-github-style] .anchor:focus { outline: none; }\n.markdown-preview[data-use-github-style] h1, .markdown-preview[data-use-github-style] h2, .markdown-preview[data-use-github-style] h3, .markdown-preview[data-use-github-style] h4, .markdown-preview[data-use-github-style] h5, .markdown-preview[data-use-github-style] h6 { position: relative; margin-top: 1em; margin-bottom: 16px; font-weight: bold; line-height: 1.4; }\n.markdown-preview[data-use-github-style] h1 .octicon-link, .markdown-preview[data-use-github-style] h2 .octicon-link, .markdown-preview[data-use-github-style] h3 .octicon-link, .markdown-preview[data-use-github-style] h4 .octicon-link, .markdown-preview[data-use-github-style] h5 .octicon-link, .markdown-preview[data-use-github-style] h6 .octicon-link { display: none; color: rgb(0, 0, 0); vertical-align: middle; }\n.markdown-preview[data-use-github-style] h1:hover .anchor, .markdown-preview[data-use-github-style] h2:hover .anchor, .markdown-preview[data-use-github-style] h3:hover .anchor, .markdown-preview[data-use-github-style] h4:hover .anchor, .markdown-preview[data-use-github-style] h5:hover .anchor, .markdown-preview[data-use-github-style] h6:hover .anchor { padding-left: 8px; margin-left: -30px; text-decoration: none; }\n.markdown-preview[data-use-github-style] h1:hover .anchor .octicon-link, .markdown-preview[data-use-github-style] h2:hover .anchor .octicon-link, .markdown-preview[data-use-github-style] h3:hover .anchor .octicon-link, .markdown-preview[data-use-github-style] h4:hover .anchor .octicon-link, .markdown-preview[data-use-github-style] h5:hover .anchor .octicon-link, .markdown-preview[data-use-github-style] h6:hover .anchor .octicon-link { display: inline-block; }\n.markdown-preview[data-use-github-style] h1 tt, .markdown-preview[data-use-github-style] h2 tt, .markdown-preview[data-use-github-style] h3 tt, .markdown-preview[data-use-github-style] h4 tt, .markdown-preview[data-use-github-style] h5 tt, .markdown-preview[data-use-github-style] h6 tt, .markdown-preview[data-use-github-style] h1 code, .markdown-preview[data-use-github-style] h2 code, .markdown-preview[data-use-github-style] h3 code, .markdown-preview[data-use-github-style] h4 code, .markdown-preview[data-use-github-style] h5 code, .markdown-preview[data-use-github-style] h6 code { font-size: inherit; }\n.markdown-preview[data-use-github-style] h1 { padding-bottom: 0.3em; font-size: 2.25em; line-height: 1.2; border-bottom: 1px solid rgb(238, 238, 238); }\n.markdown-preview[data-use-github-style] h1 .anchor { line-height: 1; }\n.markdown-preview[data-use-github-style] h2 { padding-bottom: 0.3em; font-size: 1.75em; line-height: 1.225; border-bottom: 1px solid rgb(238, 238, 238); }\n.markdown-preview[data-use-github-style] h2 .anchor { line-height: 1; }\n.markdown-preview[data-use-github-style] h3 { font-size: 1.5em; line-height: 1.43; }\n.markdown-preview[data-use-github-style] h3 .anchor { line-height: 1.2; }\n.markdown-preview[data-use-github-style] h4 { font-size: 1.25em; }\n.markdown-preview[data-use-github-style] h4 .anchor { line-height: 1.2; }\n.markdown-preview[data-use-github-style] h5 { font-size: 1em; }\n.markdown-preview[data-use-github-style] h5 .anchor { line-height: 1.1; }\n.markdown-preview[data-use-github-style] h6 { font-size: 1em; color: rgb(119, 119, 119); }\n.markdown-preview[data-use-github-style] h6 .anchor { line-height: 1.1; }\n.markdown-preview[data-use-github-style] p, .markdown-preview[data-use-github-style] blockquote, .markdown-preview[data-use-github-style] ul, .markdown-preview[data-use-github-style] ol, .markdown-preview[data-use-github-style] dl, .markdown-preview[data-use-github-style] table, .markdown-preview[data-use-github-style] pre { margin-top: 0px; margin-bottom: 16px; }\n.markdown-preview[data-use-github-style] hr { height: 4px; padding: 0px; margin: 16px 0px; border: 0px none; background-color: rgb(231, 231, 231); }\n.markdown-preview[data-use-github-style] ul, .markdown-preview[data-use-github-style] ol { padding-left: 2em; }\n.markdown-preview[data-use-github-style] ul.no-list, .markdown-preview[data-use-github-style] ol.no-list { padding: 0px; list-style-type: none; }\n.markdown-preview[data-use-github-style] ul ul, .markdown-preview[data-use-github-style] ul ol, .markdown-preview[data-use-github-style] ol ol, .markdown-preview[data-use-github-style] ol ul { margin-top: 0px; margin-bottom: 0px; }\n.markdown-preview[data-use-github-style] li > p { margin-top: 16px; }\n.markdown-preview[data-use-github-style] dl { padding: 0px; }\n.markdown-preview[data-use-github-style] dl dt { padding: 0px; margin-top: 16px; font-size: 1em; font-style: italic; font-weight: bold; }\n.markdown-preview[data-use-github-style] dl dd { padding: 0px 16px; margin-bottom: 16px; }\n.markdown-preview[data-use-github-style] blockquote { padding: 0px 15px; color: rgb(119, 119, 119); border-left: 4px solid rgb(221, 221, 221); }\n.markdown-preview[data-use-github-style] blockquote > :first-child { margin-top: 0px; }\n.markdown-preview[data-use-github-style] blockquote > :last-child { margin-bottom: 0px; }\n.markdown-preview[data-use-github-style] table { display: block; width: 100%; overflow: auto; word-break: keep-all; }\n.markdown-preview[data-use-github-style] table th { font-weight: bold; }\n.markdown-preview[data-use-github-style] table th, .markdown-preview[data-use-github-style] table td { padding: 6px 13px; border: 1px solid rgb(221, 221, 221); }\n.markdown-preview[data-use-github-style] table tr { border-top: 1px solid rgb(204, 204, 204); background-color: rgb(255, 255, 255); }\n.markdown-preview[data-use-github-style] table tr:nth-child(2n) { background-color: rgb(248, 248, 248); }\n.markdown-preview[data-use-github-style] img { max-width: 100%; box-sizing: border-box; }\n.markdown-preview[data-use-github-style] .emoji { max-width: none; }\n.markdown-preview[data-use-github-style] span.frame { display: block; overflow: hidden; }\n.markdown-preview[data-use-github-style] span.frame > span { display: block; float: left; width: auto; padding: 7px; margin: 13px 0px 0px; overflow: hidden; border: 1px solid rgb(221, 221, 221); }\n.markdown-preview[data-use-github-style] span.frame span img { display: block; float: left; }\n.markdown-preview[data-use-github-style] span.frame span span { display: block; padding: 5px 0px 0px; clear: both; color: rgb(51, 51, 51); }\n.markdown-preview[data-use-github-style] span.align-center { display: block; overflow: hidden; clear: both; }\n.markdown-preview[data-use-github-style] span.align-center > span { display: block; margin: 13px auto 0px; overflow: hidden; text-align: center; }\n.markdown-preview[data-use-github-style] span.align-center span img { margin: 0px auto; text-align: center; }\n.markdown-preview[data-use-github-style] span.align-right { display: block; overflow: hidden; clear: both; }\n.markdown-preview[data-use-github-style] span.align-right > span { display: block; margin: 13px 0px 0px; overflow: hidden; text-align: right; }\n.markdown-preview[data-use-github-style] span.align-right span img { margin: 0px; text-align: right; }\n.markdown-preview[data-use-github-style] span.float-left { display: block; float: left; margin-right: 13px; overflow: hidden; }\n.markdown-preview[data-use-github-style] span.float-left span { margin: 13px 0px 0px; }\n.markdown-preview[data-use-github-style] span.float-right { display: block; float: right; margin-left: 13px; overflow: hidden; }\n.markdown-preview[data-use-github-style] span.float-right > span { display: block; margin: 13px auto 0px; overflow: hidden; text-align: right; }\n.markdown-preview[data-use-github-style] code, .markdown-preview[data-use-github-style] tt { padding: 0.2em 0px; margin: 0px; font-size: 85%; border-radius: 3px; background-color: rgba(0, 0, 0, 0.0392157); }\n.markdown-preview[data-use-github-style] code::before, .markdown-preview[data-use-github-style] tt::before, .markdown-preview[data-use-github-style] code::after, .markdown-preview[data-use-github-style] tt::after { letter-spacing: -0.2em; content: \" \"; }\n.markdown-preview[data-use-github-style] code br, .markdown-preview[data-use-github-style] tt br { display: none; }\n.markdown-preview[data-use-github-style] del code { text-decoration: inherit; }\n.markdown-preview[data-use-github-style] pre > code { padding: 0px; margin: 0px; font-size: 100%; word-break: normal; white-space: pre; border: 0px; background: transparent; }\n.markdown-preview[data-use-github-style] .highlight { margin-bottom: 16px; }\n.markdown-preview[data-use-github-style] .highlight pre, .markdown-preview[data-use-github-style] pre { padding: 16px; overflow: auto; font-size: 85%; line-height: 1.45; border-radius: 3px; background-color: rgb(247, 247, 247); }\n.markdown-preview[data-use-github-style] .highlight pre { margin-bottom: 0px; word-break: normal; }\n.markdown-preview[data-use-github-style] pre { word-wrap: normal; }\n.markdown-preview[data-use-github-style] pre code, .markdown-preview[data-use-github-style] pre tt { display: inline; max-width: initial; padding: 0px; margin: 0px; overflow: initial; line-height: inherit; word-wrap: normal; border: 0px; background-color: transparent; }\n.markdown-preview[data-use-github-style] pre code::before, .markdown-preview[data-use-github-style] pre tt::before, .markdown-preview[data-use-github-style] pre code::after, .markdown-preview[data-use-github-style] pre tt::after { content: normal; }\n.markdown-preview[data-use-github-style] kbd { display: inline-block; padding: 3px 5px; font-size: 11px; line-height: 10px; color: rgb(85, 85, 85); vertical-align: middle; border-width: 1px; border-style: solid; border-color: rgb(204, 204, 204) rgb(204, 204, 204) rgb(187, 187, 187); border-radius: 3px; box-shadow: rgb(187, 187, 187) 0px -1px 0px inset; background-color: rgb(252, 252, 252); }\n.markdown-preview[data-use-github-style] a { color: rgb(51, 122, 183); }\n.markdown-preview[data-use-github-style] code { color: inherit; }\n.markdown-preview[data-use-github-style] pre.editor-colors { padding: 0.8em 1em; margin-bottom: 1em; font-size: 0.85em; border-radius: 4px; overflow: auto; }\n.scrollbars-visible-always .markdown-preview pre.editor-colors .vertical-scrollbar, .scrollbars-visible-always .markdown-preview pre.editor-colors .horizontal-scrollbar { visibility: hidden; }\n.scrollbars-visible-always .markdown-preview pre.editor-colors:hover .vertical-scrollbar, .scrollbars-visible-always .markdown-preview pre.editor-colors:hover .horizontal-scrollbar { visibility: visible; }\n.markdown-preview .task-list-item-checkbox { position: absolute; margin: 0.25em 0px 0px -1.4em; }\n.bracket-matcher .region {\n  border-bottom: 1px dotted lime;\n  position: absolute;\n}\n\n.spell-check-misspelling .region {\n  border-bottom: 2px dotted rgba(255, 51, 51, 0.75);\n}\n.spell-check-corrections {\n  width: 25em !important;\n}\n\npre.editor-colors {\n  background-color: #282c34;\n  color: #abb2bf;\n}\npre.editor-colors .line.cursor-line {\n  background-color: rgba(153, 187, 255, 0.04);\n}\npre.editor-colors .invisible {\n  color: #abb2bf;\n}\npre.editor-colors .cursor {\n  border-left: 2px solid #528bff;\n}\npre.editor-colors .selection .region {\n  background-color: #3e4451;\n}\npre.editor-colors .bracket-matcher .region {\n  border-bottom: 1px solid #528bff;\n  box-sizing: border-box;\n}\npre.editor-colors .invisible-character {\n  color: rgba(171, 178, 191, 0.15);\n}\npre.editor-colors .indent-guide {\n  color: rgba(171, 178, 191, 0.15);\n}\npre.editor-colors .wrap-guide {\n  background-color: rgba(171, 178, 191, 0.15);\n}\npre.editor-colors .find-result .region.region.region,\npre.editor-colors .current-result .region.region.region {\n  border-radius: 2px;\n  background-color: rgba(82, 139, 255, 0.24);\n  transition: border-color 0.4s;\n}\npre.editor-colors .find-result .region.region.region {\n  border: 2px solid transparent;\n}\npre.editor-colors .current-result .region.region.region {\n  border: 2px solid #528bff;\n  transition-duration: .1s;\n}\npre.editor-colors .gutter .line-number {\n  color: #636d83;\n  -webkit-font-smoothing: antialiased;\n}\npre.editor-colors .gutter .line-number.cursor-line {\n  color: #abb2bf;\n  background-color: #2c313a;\n}\npre.editor-colors .gutter .line-number.cursor-line-no-selection {\n  background-color: transparent;\n}\npre.editor-colors .gutter .line-number .icon-right {\n  color: #abb2bf;\n}\npre.editor-colors .gutter:not(.git-diff-icon) .line-number.git-line-removed.git-line-removed::before {\n  bottom: -3px;\n}\npre.editor-colors .gutter:not(.git-diff-icon) .line-number.git-line-removed::after {\n  content: \"\";\n  position: absolute;\n  left: 0px;\n  bottom: 0px;\n  width: 25px;\n  border-bottom: 1px dotted rgba(224, 82, 82, 0.5);\n  pointer-events: none;\n}\npre.editor-colors .gutter .line-number.folded,\npre.editor-colors .gutter .line-number:after,\npre.editor-colors .fold-marker:after {\n  color: #abb2bf;\n}\n.syntax--comment {\n  color: #5c6370;\n  font-style: italic;\n}\n.syntax--comment .syntax--markup.syntax--link {\n  color: #5c6370;\n}\n.syntax--entity.syntax--name.syntax--type {\n  color: #e5c07b;\n}\n.syntax--entity.syntax--other.syntax--inherited-class {\n  color: #98c379;\n}\n.syntax--keyword {\n  color: #c678dd;\n}\n.syntax--keyword.syntax--control {\n  color: #c678dd;\n}\n.syntax--keyword.syntax--operator {\n  color: #abb2bf;\n}\n.syntax--keyword.syntax--other.syntax--special-method {\n  color: #61afef;\n}\n.syntax--keyword.syntax--other.syntax--unit {\n  color: #d19a66;\n}\n.syntax--storage {\n  color: #c678dd;\n}\n.syntax--storage.syntax--type.syntax--annotation,\n.syntax--storage.syntax--type.syntax--primitive {\n  color: #c678dd;\n}\n.syntax--storage.syntax--modifier.syntax--package,\n.syntax--storage.syntax--modifier.syntax--import {\n  color: #abb2bf;\n}\n.syntax--constant {\n  color: #d19a66;\n}\n.syntax--constant.syntax--variable {\n  color: #d19a66;\n}\n.syntax--constant.syntax--character.syntax--escape {\n  color: #56b6c2;\n}\n.syntax--constant.syntax--numeric {\n  color: #d19a66;\n}\n.syntax--constant.syntax--other.syntax--color {\n  color: #56b6c2;\n}\n.syntax--constant.syntax--other.syntax--symbol {\n  color: #56b6c2;\n}\n.syntax--variable {\n  color: #e06c75;\n}\n.syntax--variable.syntax--interpolation {\n  color: #be5046;\n}\n.syntax--variable.syntax--parameter {\n  color: #abb2bf;\n}\n.syntax--string {\n  color: #98c379;\n}\n.syntax--string.syntax--regexp {\n  color: #56b6c2;\n}\n.syntax--string.syntax--regexp .syntax--source.syntax--ruby.syntax--embedded {\n  color: #e5c07b;\n}\n.syntax--string.syntax--other.syntax--link {\n  color: #e06c75;\n}\n.syntax--punctuation.syntax--definition.syntax--comment {\n  color: #5c6370;\n}\n.syntax--punctuation.syntax--definition.syntax--method-parameters,\n.syntax--punctuation.syntax--definition.syntax--function-parameters,\n.syntax--punctuation.syntax--definition.syntax--parameters,\n.syntax--punctuation.syntax--definition.syntax--separator,\n.syntax--punctuation.syntax--definition.syntax--seperator,\n.syntax--punctuation.syntax--definition.syntax--array {\n  color: #abb2bf;\n}\n.syntax--punctuation.syntax--definition.syntax--heading,\n.syntax--punctuation.syntax--definition.syntax--identity {\n  color: #61afef;\n}\n.syntax--punctuation.syntax--definition.syntax--bold {\n  color: #e5c07b;\n  font-weight: bold;\n}\n.syntax--punctuation.syntax--definition.syntax--italic {\n  color: #c678dd;\n  font-style: italic;\n}\n.syntax--punctuation.syntax--section.syntax--embedded {\n  color: #be5046;\n}\n.syntax--punctuation.syntax--section.syntax--method,\n.syntax--punctuation.syntax--section.syntax--class,\n.syntax--punctuation.syntax--section.syntax--inner-class {\n  color: #abb2bf;\n}\n.syntax--support.syntax--class {\n  color: #e5c07b;\n}\n.syntax--support.syntax--type {\n  color: #56b6c2;\n}\n.syntax--support.syntax--function {\n  color: #56b6c2;\n}\n.syntax--support.syntax--function.syntax--any-method {\n  color: #61afef;\n}\n.syntax--entity.syntax--name.syntax--function {\n  color: #61afef;\n}\n.syntax--entity.syntax--name.syntax--class,\n.syntax--entity.syntax--name.syntax--type.syntax--class {\n  color: #e5c07b;\n}\n.syntax--entity.syntax--name.syntax--section {\n  color: #61afef;\n}\n.syntax--entity.syntax--name.syntax--tag {\n  color: #e06c75;\n}\n.syntax--entity.syntax--other.syntax--attribute-name {\n  color: #d19a66;\n}\n.syntax--entity.syntax--other.syntax--attribute-name.syntax--id {\n  color: #61afef;\n}\n.syntax--meta.syntax--class {\n  color: #e5c07b;\n}\n.syntax--meta.syntax--class.syntax--body {\n  color: #abb2bf;\n}\n.syntax--meta.syntax--method-call,\n.syntax--meta.syntax--method {\n  color: #abb2bf;\n}\n.syntax--meta.syntax--definition.syntax--variable {\n  color: #e06c75;\n}\n.syntax--meta.syntax--link {\n  color: #d19a66;\n}\n.syntax--meta.syntax--require {\n  color: #61afef;\n}\n.syntax--meta.syntax--selector {\n  color: #c678dd;\n}\n.syntax--meta.syntax--separator {\n  background-color: #373b41;\n  color: #abb2bf;\n}\n.syntax--meta.syntax--tag {\n  color: #abb2bf;\n}\n.syntax--underline {\n  text-decoration: underline;\n}\n.syntax--none {\n  color: #abb2bf;\n}\n.syntax--invalid.syntax--deprecated {\n  color: #523d14 !important;\n  background-color: #e0c285 !important;\n}\n.syntax--invalid.syntax--illegal {\n  color: white !important;\n  background-color: #e05252 !important;\n}\n.syntax--markup.syntax--bold {\n  color: #d19a66;\n  font-weight: bold;\n}\n.syntax--markup.syntax--changed {\n  color: #c678dd;\n}\n.syntax--markup.syntax--deleted {\n  color: #e06c75;\n}\n.syntax--markup.syntax--italic {\n  color: #c678dd;\n  font-style: italic;\n}\n.syntax--markup.syntax--heading {\n  color: #e06c75;\n}\n.syntax--markup.syntax--heading .syntax--punctuation.syntax--definition.syntax--heading {\n  color: #61afef;\n}\n.syntax--markup.syntax--link {\n  color: #56b6c2;\n}\n.syntax--markup.syntax--inserted {\n  color: #98c379;\n}\n.syntax--markup.syntax--quote {\n  color: #d19a66;\n}\n.syntax--markup.syntax--raw {\n  color: #98c379;\n}\n.syntax--source.syntax--c .syntax--keyword.syntax--operator {\n  color: #c678dd;\n}\n.syntax--source.syntax--cpp .syntax--keyword.syntax--operator {\n  color: #c678dd;\n}\n.syntax--source.syntax--cs .syntax--keyword.syntax--operator {\n  color: #c678dd;\n}\n.syntax--source.syntax--css .syntax--property-name,\n.syntax--source.syntax--css .syntax--property-value {\n  color: #828997;\n}\n.syntax--source.syntax--css .syntax--property-name.syntax--support,\n.syntax--source.syntax--css .syntax--property-value.syntax--support {\n  color: #abb2bf;\n}\n.syntax--source.syntax--gfm .syntax--markup {\n  -webkit-font-smoothing: auto;\n}\n.syntax--source.syntax--gfm .syntax--link .syntax--entity {\n  color: #61afef;\n}\n.syntax--source.syntax--go .syntax--storage.syntax--type.syntax--string {\n  color: #c678dd;\n}\n.syntax--source.syntax--ini .syntax--keyword.syntax--other.syntax--definition.syntax--ini {\n  color: #e06c75;\n}\n.syntax--source.syntax--java .syntax--storage.syntax--modifier.syntax--import {\n  color: #e5c07b;\n}\n.syntax--source.syntax--java .syntax--storage.syntax--type {\n  color: #e5c07b;\n}\n.syntax--source.syntax--java .syntax--keyword.syntax--operator.syntax--instanceof {\n  color: #c678dd;\n}\n.syntax--source.syntax--java-properties .syntax--meta.syntax--key-pair {\n  color: #e06c75;\n}\n.syntax--source.syntax--java-properties .syntax--meta.syntax--key-pair > .syntax--punctuation {\n  color: #abb2bf;\n}\n.syntax--source.syntax--js .syntax--keyword.syntax--operator {\n  color: #56b6c2;\n}\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--delete,\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--in,\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--of,\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--instanceof,\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--new,\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--typeof,\n.syntax--source.syntax--js .syntax--keyword.syntax--operator.syntax--void {\n  color: #c678dd;\n}\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--dictionary.syntax--json > .syntax--string.syntax--quoted.syntax--json {\n  color: #e06c75;\n}\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--dictionary.syntax--json > .syntax--string.syntax--quoted.syntax--json > .syntax--punctuation.syntax--string {\n  color: #e06c75;\n}\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--dictionary.syntax--json > .syntax--value.syntax--json > .syntax--string.syntax--quoted.syntax--json,\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--array.syntax--json > .syntax--value.syntax--json > .syntax--string.syntax--quoted.syntax--json,\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--dictionary.syntax--json > .syntax--value.syntax--json > .syntax--string.syntax--quoted.syntax--json > .syntax--punctuation,\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--array.syntax--json > .syntax--value.syntax--json > .syntax--string.syntax--quoted.syntax--json > .syntax--punctuation {\n  color: #98c379;\n}\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--dictionary.syntax--json > .syntax--constant.syntax--language.syntax--json,\n.syntax--source.syntax--json .syntax--meta.syntax--structure.syntax--array.syntax--json > .syntax--constant.syntax--language.syntax--json {\n  color: #56b6c2;\n}\n.syntax--source.syntax--ruby .syntax--constant.syntax--other.syntax--symbol > .syntax--punctuation {\n  color: inherit;\n}\n.syntax--source.syntax--python .syntax--keyword.syntax--operator.syntax--logical.syntax--python {\n  color: #c678dd;\n}\n.syntax--source.syntax--python .syntax--variable.syntax--parameter {\n  color: #d19a66;\n}\n</style>\n  </head>\n  <body class='markdown-preview' data-use-github-style><h1 id=\"an-h1-header\">An h1 header</h1>\n<p>Paragraphs are separated by a blank line.</p>\n<p>2nd paragraph. <em>Italic</em>, <strong>bold</strong>, and <code>monospace</code>. Itemized lists\nlook like:</p>\n<ul>\n<li>this one</li>\n<li>that one</li>\n<li>the other one</li>\n</ul>\n<p>Note that --- not considering the asterisk --- the actual text\ncontent starts at 4-columns in.</p>\n<blockquote>\n<p>Block quotes are\nwritten like so.</p>\n<p>They can span multiple paragraphs,\nif you like.</p>\n</blockquote>\n<p>Use 3 dashes for an em-dash. Use 2 dashes for ranges (ex., &quot;it&#39;s all\nin chapters 12--14&quot;). Three dots ... will be converted to an ellipsis.\nUnicode is supported. ?</p>\n<h2 id=\"an-h2-header\">An h2 header</h2>\n<p>Here&#39;s a numbered list:</p>\n<ol>\n<li>first item</li>\n<li>second item</li>\n<li>third item</li>\n</ol>\n<p>Note again how the actual text starts at 4 columns in (4 characters\nfrom the left side). Here&#39;s a code sample:</p>\n<pre class=\"editor-colors lang-text\"><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>#&nbsp;Let&nbsp;me&nbsp;re-iterate&nbsp;...</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>for&nbsp;i&nbsp;in&nbsp;1&nbsp;..&nbsp;10&nbsp;{&nbsp;do-something(i)&nbsp;}</span></span></span></div></pre><p>As you probably guessed, indented 4 spaces. By the way, instead of\nindenting the block, you can use delimited blocks, if you like:</p>\n<pre class=\"editor-colors lang-text\"><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>define&nbsp;foobar()&nbsp;{</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>print&nbsp;&quot;Welcome&nbsp;to&nbsp;flavor&nbsp;country!&quot;;</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>}</span></span></span></div></pre><p>(which makes copying &amp; pasting easier). You can optionally mark the\ndelimited block for Pandoc to syntax highlight it:</p>\n<pre class=\"editor-colors lang-python\"><div class=\"line\"><span class=\"syntax--source syntax--python\"><span class=\"syntax--keyword syntax--control syntax--import syntax--python\"><span>import</span></span><span>&nbsp;</span><span>time</span></span></div><div class=\"line\"><span class=\"syntax--source syntax--python\"><span class=\"syntax--comment syntax--line syntax--number-sign syntax--python\"><span class=\"syntax--punctuation syntax--definition syntax--comment syntax--python\"><span>#</span></span><span>&nbsp;Quick,&nbsp;count&nbsp;to&nbsp;ten!</span><span>&nbsp;</span></span></span></div><div class=\"line\"><span class=\"syntax--source syntax--python\"><span class=\"syntax--keyword syntax--control syntax--repeat syntax--python\"><span>for</span></span><span>&nbsp;</span><span>i</span><span>&nbsp;</span><span class=\"syntax--keyword syntax--operator syntax--logical syntax--python\"><span>in</span></span><span>&nbsp;</span><span class=\"syntax--meta syntax--function-call syntax--python\"><span class=\"syntax--support syntax--function syntax--builtin syntax--python\"><span>range</span></span><span class=\"syntax--punctuation syntax--definition syntax--arguments syntax--begin syntax--python\"><span>(</span></span><span class=\"syntax--meta syntax--function-call syntax--arguments syntax--python\"><span class=\"syntax--constant syntax--numeric syntax--integer syntax--decimal syntax--python\"><span>10</span></span></span><span class=\"syntax--punctuation syntax--definition syntax--arguments syntax--end syntax--python\"><span>)</span></span></span><span>:</span></span></div><div class=\"line\"><span class=\"syntax--source syntax--python\"><span class=\"syntax--punctuation syntax--whitespace syntax--comment syntax--leading syntax--python\"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span></span><span class=\"syntax--comment syntax--line syntax--number-sign syntax--python\"><span class=\"syntax--punctuation syntax--definition syntax--comment syntax--python\"><span>#</span></span><span>&nbsp;(but&nbsp;not&nbsp;*too*&nbsp;quick)</span><span>&nbsp;</span></span></span></div><div class=\"line\"><span class=\"syntax--source syntax--python\"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class=\"syntax--meta syntax--function-call syntax--python\"><span>time</span><span>.</span><span>sleep</span><span class=\"syntax--punctuation syntax--definition syntax--arguments syntax--begin syntax--python\"><span>(</span></span><span class=\"syntax--meta syntax--function-call syntax--arguments syntax--python\"><span class=\"syntax--constant syntax--numeric syntax--float syntax--python\"><span>0.5</span></span></span><span class=\"syntax--punctuation syntax--definition syntax--arguments syntax--end syntax--python\"><span>)</span></span></span></span></div><div class=\"line\"><span class=\"syntax--source syntax--python\"><span>&nbsp;&nbsp;&nbsp;&nbsp;</span><span class=\"syntax--keyword syntax--other syntax--python\"><span>print</span></span><span>&nbsp;</span><span>i</span></span></div></pre>\n<h3 id=\"an-h3-header\">An h3 header</h3>\n<p>Now a nested list:</p>\n<ol>\n<li><p>First, get these ingredients:</p>\n<ul>\n<li>carrots</li>\n<li>celery</li>\n<li>lentils</li>\n</ul>\n</li>\n<li><p>Boil some water.</p>\n</li>\n<li><p>Dump everything in the pot and follow\nthis algorithm:</p>\n<pre class=\"editor-colors lang-text\"><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>find&nbsp;wooden&nbsp;spoon</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>uncover&nbsp;pot</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>stir</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>cover&nbsp;pot</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>balance&nbsp;wooden&nbsp;spoon&nbsp;precariously&nbsp;on&nbsp;pot&nbsp;handle</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>wait&nbsp;10&nbsp;minutes</span></span></span></div><div class=\"line\"><span class=\"syntax--text syntax--plain\"><span class=\"syntax--meta syntax--paragraph syntax--text\"><span>goto&nbsp;first&nbsp;step&nbsp;(or&nbsp;shut&nbsp;off&nbsp;burner&nbsp;when&nbsp;done)</span></span></span></div></pre><p>Do not bump wooden spoon or it will fall.</p>\n</li>\n</ol>\n<p>Notice again how text always lines up on 4-space indents (including\nthat last line which continues item 3 above).</p>\n<p>Here&#39;s a link to <a href=\"http://foo.bar\">a website</a>, to a <a href=\"local-doc.html\">local\ndoc</a>, and to a <a href=\"#an-h2-header\">section heading in the current\ndoc</a>. Here&#39;s a footnote [^1].</p>\n<p>[^1]: Footnote text goes here.</p>\n<p>Tables can look like this:</p>\n<p>size  material      color</p>\n<hr>\n<p>9     leather       brown\n10    hemp canvas   natural\n11    glass         transparent</p>\n<p>Table: Shoes, their sizes, and what they&#39;re made of</p>\n<p>(The above is the caption for the table.) Pandoc also supports\nmulti-line tables:</p>\n<hr>\n<p>keyword   text</p>\n<hr>\n<p>red       Sunsets, apples, and\n          other red or reddish\n          things.</p>\n<p>green     Leaves, grass, frogs\n          and other things it&#39;s\n          not easy being.</p>\n<hr>\n<p>A horizontal rule follows.</p>\n<hr>\n<p>Here&#39;s a definition list:</p>\n<p>apples\n  : Good for making applesauce.\noranges\n  : Citrus!\ntomatoes\n  : There&#39;s no &quot;e&quot; in tomatoe.</p>\n<p>Again, text is indented 4 spaces. (Put a blank line between each\nterm/definition pair to spread things out more.)</p>\n<p>Here&#39;s a &quot;line block&quot;:</p>\n<p>| Line one\n|   Line too\n| Line tree</p>\n<p>and images can be specified like so:</p>\n<p><img src=\"C:\\Users\\Christian\\Desktop\\example-image.jpg\" alt=\"example image\" title=\"An exemplary image\"></p>\n<p>Inline math equations go in like so: $\\omega = d\\phi / dt$. Display\nmath should get its own line and be put in in double-dollarsigns:</p>\n<p>$$I = \\int \\rho R^{2} dV$$</p>\n<p>And note that you can backslash-escape any punctuation characters\nwhich you wish to be displayed literally, ex.: `foo`, *bar*, etc.</p></body>\n</html>\n";

            }

            return myLessonHTML;


            //Byte[] res = AgenaTrader.UserCode.Properties.Resources.test_markdown_file;
            //Byte [] res = Properties.Resources.test_markdown_file;

            //System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            //string resourceName = asm.GetName().Name + ".Properties.Resources";
            //var rm = new System.Resources.ResourceManager(resourceName, asm);
            //Byte[] res = (Byte [])rm.GetObject(FileName);
            //string myHTML = (string)rm.GetObject(FileName);

            //string s_unicode2 = System.Text.Encoding.UTF8.GetString(res);
            //var myMDHtml = Markdown.ToHtml(s_unicode2);

            //return myMDHtml;
            //return s_unicode2;
            //string myHTML = Properties.Resources.test_markdown_file_html_md;
            //return myHTML;
        }

        public override void OnPaint(Graphics g, Rectangle r, double min, double max)
        {

            using (Font font1 = new Font("Arial", 8, FontStyle.Bold, GraphicsUnit.Point))
            {
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                this.Core.GetDataDirectory();

                Brush tempbrush = new SolidBrush(Color.Black);

                _rect = new RectangleF(r.Width - 100, 80, 86, 27);
                g.FillRectangle(tempbrush, _rect);
                g.DrawString("ScriptAcademy", font1, Brushes.White, _rect, stringFormat);
            }
        }

        private void OnChartPanelMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            Point cursorPos = new Point(e.X, e.Y);
            if (_rect.Contains(cursorPos))
            {
                if (myHTML == "")
                {
                    myHTML = getResource("test_markdown_file_html_md");
                }
                showLesson(myHTML);
            }
        }

        public void showLesson(string Htlm)
        {

            WebBrowser wb_lesson = new System.Windows.Forms.WebBrowser();
            //SuspendLayout();
            // 
            // wb_lesson
            // 
            wb_lesson.Dock = System.Windows.Forms.DockStyle.Fill;
            wb_lesson.Location = new System.Drawing.Point(0, 0);
            wb_lesson.MinimumSize = new System.Drawing.Size(20, 20);
            wb_lesson.Name = "wb_lesson";
            wb_lesson.Size = new System.Drawing.Size(532, 398);
            wb_lesson.TabIndex = 0;


            // 
            // frm_lesson
            // 
            if (_frm_lesson == null || _frm_lesson.IsDisposed == true)
            {

                _frm_lesson = new Form();

                Size ClientSize = new System.Drawing.Size(500, 400);
                _frm_lesson.Controls.Add(wb_lesson);
                _frm_lesson.Text = "ScriptAcademy Lesson 1 Unit 1";
                _frm_lesson.Name = "frm_lesson";
                _frm_lesson.Size = ClientSize;
            }



            wb_lesson.DocumentText = Htlm;
            
            //TODO kein sperrender Dialog
            _frm_lesson.ShowDialog();

        }


    }
}
#region AgenaTrader Automaticaly Generated Code. Do not change it manualy

namespace AgenaTrader.UserCode
{
	#region Indicator

	public partial class UserIndicator
	{
		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy()
        {
			return ScriptAcademy(InSeries);
		}

		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy(IDataSeries input)
		{
			var indicator = CachedCalculationUnits.GetCachedIndicator<ScriptAcademy>(input);

			if (indicator != null)
				return indicator;

			indicator = new ScriptAcademy
						{
							RequiredBarsCount = RequiredBarsCount,
							CalculateOnClosedBar = CalculateOnClosedBar,
							InSeries = input
						};
			indicator.SetUp();

			CachedCalculationUnits.AddIndicator2Cache(indicator);

			return indicator;
		}
	}

	#endregion

	#region Strategy

	public partial class UserStrategy
	{
		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy()
		{
			return LeadIndicator.ScriptAcademy(InSeries);
		}

		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy(IDataSeries input)
		{
			if (IsInInit && input == null)
				throw new ArgumentException("You only can access an indicator with the default input/bar series from within the 'OnInit()' method");

			return LeadIndicator.ScriptAcademy(input);
		}
	}

	#endregion

	#region Column

	public partial class UserColumn
	{
		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy()
		{
			return LeadIndicator.ScriptAcademy(InSeries);
		}

		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy(IDataSeries input)
		{
			return LeadIndicator.ScriptAcademy(input);
		}
	}

	#endregion

	#region Scripted Condition

	public partial class UserScriptedCondition
	{
		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy()
		{
			return LeadIndicator.ScriptAcademy(InSeries);
		}

		/// <summary>
		/// ScriptAcademy
		/// </summary>
		public ScriptAcademy ScriptAcademy(IDataSeries input)
		{
			return LeadIndicator.ScriptAcademy(input);
		}
	}

	#endregion

}

#endregion
