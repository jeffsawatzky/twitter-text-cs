using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;
using System;

namespace Twitter.Text
{
    public abstract class ConformanceTests
    {
        private string __YmlFile;

        protected ConformanceTests(string ymlFile)
        {
            this.__YmlFile = ymlFile;
        }

        /// <summary>
        /// Really ugly code to parse the YAML files...
        /// </summary>
        /// <typeparam name="TExpected">The type that the expected value is supposed to be</typeparam>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected IList<dynamic> LoadTestSection<TExpected>(string sectionName)
        {
            using (StreamReader stream = new StreamReader(Path.Combine("twitter-text-conformance", __YmlFile)))
            {
                YamlStream yaml = new YamlStream();
                yaml.Load(stream);

                // load the 'root' yaml node
                YamlMappingNode root = yaml.Documents[0].RootNode as YamlMappingNode;
                if (root != null)
                {
                    // load the 'tests' node
                    YamlMappingNode tests = root.Children[new YamlScalarNode("tests")] as YamlMappingNode;
                    if (tests != null)
                    {
                        // go through each section in the 'tests' node looking for the one that matches
                        // the given section name
                        foreach (KeyValuePair<YamlNode, YamlNode> section in tests.Children)
                        {
                            YamlScalarNode sect = section.Key as YamlScalarNode;
                            if (sect != null && sect.Value == sectionName)
                            {
                                YamlSequenceNode items = section.Value as YamlSequenceNode;
                                if (items != null)
                                {
                                    List<dynamic> list = new List<dynamic>();
                                    foreach (YamlMappingNode item in items)
                                    {
                                        dynamic test = new ExpandoObject();
                                        test.description = ConvertNode<string>(item.Children.Single(x => x.Key.ToString() == "description").Value);
                                        test.text = ConvertNode<string>(item.Children.Single(x => x.Key.ToString() == "text").Value);
                                        test.expected = ConvertNode<TExpected>(item.Children.Single(x => x.Key.ToString() == "expected").Value);
                                        test.hits = ConvertNode<List<List<int>>>(item.Children.SingleOrDefault(x => x.Key.ToString() == "hits").Value);
                                        list.Add(test);
                                    }
                                    return list;
                                }
                            }
                        }
                    }
                }
            }

            throw new InvalidOperationException(string.Format("Test section '{0}' could not be found in '{1}'", sectionName, __YmlFile));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node"></param>
        /// <returns></returns>
        private dynamic ConvertNode<T>(YamlNode node)
        {
            if (node == null)
            {
                return default(T);
            }

            dynamic dynnode = node as dynamic;

            if (node is YamlScalarNode)
            {
                if (string.IsNullOrWhiteSpace(dynnode.Value))
                {
                    return null;
                }
                else if (typeof(T) == typeof(int))
                {
                    return int.Parse(dynnode.Value);
                }
                else if (typeof(T) == typeof(bool))
                {
                    return dynnode.Value == "true";
                }
                else
                {
                    return dynnode.Value;
                }
            }
            else if (node is YamlSequenceNode)
            {
                dynamic list;
                if (typeof(T) == typeof(List<List<int>>))
                {
                    list = new List<List<int>>();
                    foreach (var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<List<int>>(item));
                    }
                }
                else if (typeof(T) == typeof(List<int>))
                {
                    list = new List<int>();
                    foreach (var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<int>(item));
                    }
                }
                else
                {
                    list = new List<dynamic>();
                    foreach (var item in dynnode.Children)
                    {
                        list.Add(ConvertNode<T>(item));
                    }
                }
                return list;
            }
            else if (node is YamlMappingNode)
            {
                dynamic mapnode = new ExpandoObject();
                foreach (var item in ((YamlMappingNode)node).Children)
                {
                    var key = item.Key.ToString();
                    if (key == "indices")
                    {
                        ((IDictionary<string, object>)mapnode).Add(key, ConvertNode<int>(item.Value));
                    }
                    else
                    {
                        ((IDictionary<string, object>)mapnode).Add(key, ConvertNode<T>(item.Value));
                    }
                }
                return mapnode;
            }
            return null;
        }
    }
}