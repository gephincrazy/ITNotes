using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlerLib
{
    public static class XPathTool
    {

        #region return single node value
        public static string GetNodeValue(string html, string xpath, string[] delPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                if (doc.DocumentNode.SelectSingleNode(xpath) != null)
                {
                    string htmlSegment = doc.DocumentNode.SelectSingleNode(xpath).OuterHtml;
                    foreach (var reg in delPattern)
                    {
                        htmlSegment = Regex.Replace(htmlSegment, reg, string.Empty).Trim();
                    }
                    return htmlSegment;

                }

            }
            catch (Exception ex)
            {

            }

            return string.Empty;
        }
        #endregion

        #region check contains xpath
        public static bool IsContains(string html, string xpath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.SelectNodes(xpath) == null ? false : true;
        }
        #endregion



        #region return all nodes value by <br/> split
        public static string GetNodesValue(string html, string xpath, string[] nonPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string nodesValue = string.Empty;
                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpath))
                    {
                        //nodesValue += Regex.Replace(node.OuterHtml, regPattern, string.Empty).Trim() + " | ";
                        var nodeHTML = node.OuterHtml;
                        foreach (var reg in nonPattern)
                        {
                            nodeHTML = Regex.Replace(nodeHTML, reg, string.Empty).Trim();
                        }
                        nodesValue += nodeHTML + " <br/> ";
                    }
                }
                return nodesValue;

            }
            catch (Exception ex) { }

            return string.Empty;

        }
        #endregion

        public static HtmlNodeCollection GetHTMLNodes(string html, string xpath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.SelectNodes(xpath);
        }

        public static string Format(string html) {
            try {
                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.LoadHtml(html);
                return doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex) {
                return string.Empty;
            }
        }

        #region return all nodes html arrary
        public static string[] GetNodes(string html, string xpath)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.OptionAutoCloseOnEnd = true;
                doc.LoadHtml(html);
                string nodesValue = string.Empty;
                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    return doc.DocumentNode.SelectNodes(xpath).Select(x => x.OuterHtml).ToArray();
                }
                return null;

            }
            catch (Exception ex) { }

            return null;
        }
        #endregion



        #region GetFerryInfo
        public static dynamic[] GetFerryInfo(string html, string xpath, string regPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    dynamic[] info = new dynamic[doc.DocumentNode.SelectNodes(xpath).Count()];
                    int index = 0;
                    foreach (HtmlNode liNode in doc.DocumentNode.SelectNodes(xpath))
                    {
                        //nodesValue += Regex.Replace(node.OuterHtml, regPattern, string.Empty).Trim() + " | ";
                        HtmlDocument liDoc = new HtmlDocument();
                        liDoc.LoadHtml(liNode.OuterHtml);

                        HtmlNode fromNode = liDoc.DocumentNode.SelectSingleNode("//li/a[1]");
                        string fromPlace = fromNode.InnerHtml;
                        string fromHref = Regex.Match(fromNode.OuterHtml, @"/[\w]+\.htm", RegexOptions.IgnoreCase).Groups[0].Value;
                        HtmlNode toNode = liDoc.DocumentNode.SelectSingleNode("//li/a[2]");
                        string toPlace = toNode.InnerHtml;
                        string toHref = Regex.Match(toNode.OuterHtml, @"/[\w]+\.htm", RegexOptions.IgnoreCase).Groups[0].Value;

                        string openTime = liDoc.DocumentNode.SelectSingleNode("//li/ul/li[1]").InnerHtml;
                        string journeyTime = liDoc.DocumentNode.SelectSingleNode("//li/ul/li[2]").InnerHtml;
                        info[index++] = new { fromPlace, fromHref, toPlace, toHref, openTime, journeyTime };
                    }
                    return info;
                }

                return null;

            }
            catch (Exception ex) { }

            return null;

        }

        #endregion

        #region Get2TagsValue
        public static string Get2TagsValue(string html, string xpath, string removeTagPattern, string firstTagPattern, string secondTagPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string nodesValue = string.Empty;
                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpath))
                    {
                        var key = Regex.Match(node.OuterHtml, firstTagPattern, RegexOptions.IgnoreCase).Groups.Count > 0 ? Regex.Match(node.OuterHtml, firstTagPattern, RegexOptions.IgnoreCase).Groups[0].Value : string.Empty;
                        key = key == string.Empty ? string.Empty : Regex.Replace(key, removeTagPattern, string.Empty).Trim();
                        var value = Regex.Match(node.OuterHtml, secondTagPattern, RegexOptions.IgnoreCase).Groups.Count > 0 ? Regex.Match(node.OuterHtml, secondTagPattern, RegexOptions.IgnoreCase).Groups[0].Value : string.Empty;
                        value = value == string.Empty ? string.Empty : Regex.Replace(value, removeTagPattern, string.Empty).Trim();

                        if (key == string.Empty) continue;
                        if (value == string.Empty)
                        {
                            nodesValue += key + " | ";
                        }
                        else
                        {
                            nodesValue += key + "(" + value + ")" + " | ";
                        }

                    }
                }
                return nodesValue;

            }
            catch (Exception ex) { }

            return string.Empty;
        }

        #endregion

        #region return value match pattern
        public static string ValueMatchPattern(string value, string regPattern)
        {
            Match match = Regex.Match(value, regPattern);
            if (match.Success)
            {
                return match.Groups[0].ToString().Trim();
            }
            return null;

        }
        #endregion

        #region return string[] nodes value match pattern
        public static string[] GetNodesMatchPattern(string html, string xpath, string matchPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string nodesValue = string.Empty;
                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    return doc.DocumentNode.SelectNodes(xpath).Select(x => ValueMatchPattern(x.OuterHtml, matchPattern)).ToArray();
                }


            }
            catch (Exception ex)
            {
            }

            return null;
        } 
        #endregion


        public static string GetNodeAttrValue(string html, string xpath, string matchPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                string nodesValue = string.Empty;
                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpath))
                    {
                        Match match = Regex.Match(node.OuterHtml, matchPattern);
                        while (match.Success)
                        {
                            nodesValue += match.Groups[0].ToString().Trim() + " | ";
                            match = match.NextMatch();
                        }
                    }
                }
                return nodesValue;

            }
            catch (Exception ex) { }

            return string.Empty;
        }

        public static string GetAttribute(string fragment, string attribute) {
            try {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(fragment);
                HtmlNode node = doc.DocumentNode.FirstChild;
                return node.GetAttributeValue(attribute, string.Empty);
            } catch (Exception ex) {
                return string.Empty;
            }
        }

        public static object[] GetNodeAttrValues(string html, string xpath, string regPattern)
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                if (doc.DocumentNode.SelectNodes(xpath) != null)
                {
                    foreach (HtmlNode node in doc.DocumentNode.SelectNodes(xpath))
                    {
                        MatchCollection matchs = Regex.Matches(node.OuterHtml, regPattern);
                        return Regex.Matches(node.OuterHtml, regPattern)
                            .Cast<Match>()
                            .Select(m => m.Value)
                            .Distinct()
                            .ToArray();
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return null;
        }
    }
}
