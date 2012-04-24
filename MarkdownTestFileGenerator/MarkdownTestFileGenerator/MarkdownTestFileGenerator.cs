using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MarkdownTestFileGenerator
{
    class MarkdownTestFileGenerator
    {
        public static List<string> paragraphs = new List<string>();
        public static List<string> headings = new List<string>();
        public static StringBuilder index = new StringBuilder();
        public static int numberOfPosts = 1;
        public static string exportDirectory = ".";
        public static int minParagraphsPerPost = 3;
        public static int maxParagraphsPerPost = 10;
        public static int numParagraphsInCurrentPost = minParagraphsPerPost;
        public static Random randy = new Random((int)DateTime.Now.Ticks);

        public static string helpText = "MarkdownTestFileGenerator numberOfPosts exportDirectory <minParagraphsPerPost> <maxParagraphsPerPost>\n" +
                                        "Prepare for Markdown Explosion";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("\n" + helpText + "\n");
                Console.WriteLine("You need to include the required parameters.\n");
                Console.WriteLine("Command line app don't care. Command line app don't give a crap.\n");
                Console.WriteLine("http://www.youtube.com/watch?v=4r7wHMg5Yjg" + "\n");
                return;
            }

            if (args[0] != null && int.TryParse(args[0], out numberOfPosts))
            {
                // Yay. TryParse are awesome. It leaves a bit of 
                // superflous residue here, but hey, yay.
            }
            else
            {
               Console.WriteLine("\n" + helpText + "\n");
               Console.WriteLine("You need to enter a number of posts that is, you know, a number." + "\n");
               return;
            }

            if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
            {
                exportDirectory = args[1];
                if (!Directory.Exists(exportDirectory + @"\posts"))
                {
                    Directory.CreateDirectory(exportDirectory + @"\posts");
                }
            }
            else
            {
                Console.WriteLine("\n" + helpText + "\n");
                Console.WriteLine("Creating 10,000 new files in Windows/system32. Nah, just kidding. You need to tell me where to put the files you want." + "\n");
                return;
            }

            if (args.Length >= 3 && args[2] != null && int.TryParse(args[2], out minParagraphsPerPost))
            {
                //Yay.
            }

            if (args.Length >= 4 && args[3] != null && int.TryParse(args[3], out maxParagraphsPerPost))
            {
                //Ditto!
            }

            if (minParagraphsPerPost > maxParagraphsPerPost)
            {
                Console.WriteLine("\n" + helpText + "\n");
                Console.WriteLine("Minimum means lowest, maximum means highest. Just FYI." + "\n");
                return;
            }

            loadLorem(File.ReadAllLines("lorem01.txt"));
            loadLorem(File.ReadAllLines("lorem02.txt"));
            loadHeadings(File.ReadAllLines("LoremHeadings.txt"));

            index.Append("{\n");

            for (int i = 0; i < numberOfPosts; i++)
            {
                makePost(i);
                index.Append("    {\n");

                index.Append("        title:\"" + "Post" + string.Format("{0:0000000}", i) + "\",\n");
                index.Append("        subtitle:\"" + string.Format("{0:0000000}", i) + " Was Awesome!\",\n");
                index.Append("        author:\"Ry Ter\",\n");
                index.Append("        path:\"" + exportDirectory + "\\posts\",\n");
                index.Append("        name:\"" + string.Format("{0:0000000}", i) + ".md" + "\",\n");
                index.Append("        pubdate:\"" + DateTime.Now.AddDays(numberOfPosts - 1).ToShortDateString() + "\",\n");
                index.Append("        category:\"blog\",\n");
                index.Append("        tags:\"good,blog,toast\",\n");
                index.Append("        slug:\"blog, blog bloggety blog.\",\n");
                index.Append("        excerpt:\"blog, blog bloggety blog.\"\n");

                index.Append("    }");
                if (i < numberOfPosts - 1)
                {
                    index.Append(",\n");
                }
            }

            index.Append("}");

            File.WriteAllText(exportDirectory + "\\siteIndex.json", index.ToString());
        }

        private static void makePost(int postNum)
        {
            numParagraphsInCurrentPost = randy.Next(minParagraphsPerPost, maxParagraphsPerPost);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("#" + headings[randy.Next(headings.Count)] + "\n");

                for (int x = 0; x < numParagraphsInCurrentPost; x++)
                {
                    sb.AppendLine(paragraphs[randy.Next(paragraphs.Count)] + "\n");
                }

                string fileName = exportDirectory + @"\posts\post" + string.Format("{0:0000000}", postNum) + ".md";
                File.WriteAllText(fileName, sb.ToString());
        }

        public static void loadLorem(string[] lorem)
        {
            foreach (var item in lorem)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    paragraphs.Add(item);
                }
            }
        }

        public static void loadHeadings(string[] headingList)
        {
            foreach (var item in headingList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    headings.Add(item.Trim('.'));
                }
            }
        }
    }
}
