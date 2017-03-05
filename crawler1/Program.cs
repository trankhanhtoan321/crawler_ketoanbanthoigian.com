using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace crawler1
{
    class Program
    {
        static void Main(string[] args)
        {
            TinTrinhLibrary.WebClient client = new TinTrinhLibrary.WebClient();
            int start = 0, dem;
            String tieude, noidung, hinhanh;
            String reg = "<a href=" + '"' + "(.*?)" + '"' + " itemprop=" + '"' + "url" + '"';
            String imgs = "<img src=" + '"' + "(.*?)" + '"' + " alt=" + '"' + '"' + " itemprop=" + '"' + "thumbnailUrl" + '"' + "/>";
            client.Post("http://dichvuketoantainha.net/login", "trankhanhtoan=b303baf471a2a712b2456ed41c822ee8&user_name=admin&user_pass=BUKT&login=Log+in", "http://dichvuketoantainha.net", "");

        loop:
            String html = client.Get("http://ketoanbanthoigian.com/kinh-nghiem-ke-toan.html?start=" + start, "http://ketoanbanthoigian.com", "");
            start = start + 10;

            List<String> imgurls = new List<String>();
            if (start > 180) goto finish;
            MatchCollection rg = Regex.Matches(html, reg);
            MatchCollection rgimg = Regex.Matches(html, imgs);
            dem = -1;
            foreach(Match rimg in rgimg)
            {
                dem++;
                String img = "http://ketoanbanthoigian.com"+rimg.Groups[1].Value;
                imgurls.Add(img);
            }
            dem = -1;
            foreach (Match r in rg)
            {
                dem++;
                String url = "http://ketoanbanthoigian.com" + r.Groups[1].Value;
                String htmli = client.Get(url, "http://ketoanbanthoigian.com", "");
                int i = htmli.IndexOf("<h2 itemprop=\"name\">");
                int j = htmli.IndexOf("</h2>", i);
                tieude = htmli.Substring(i + 20, j - i - 20).Trim();

                int i1 = htmli.IndexOf("<div itemprop=\"articleBody\">");
                int j1 = htmli.IndexOf("<div class=\"extranews_separator\"></div>",i1);
                noidung = htmli.Substring(i1 + 28, j1 - i1 - 28).Trim();

                Console.WriteLine(dem+1);
                hinhanh = imgurls.ElementAt(dem);
                /*
                 * thuc hien upload vao website dichvuketoantainha.net
                 * */
                //client.Post("http://dichvuketoantainha.net/admin/new_blog", "blog_name="+tieude+ "&blog_image="+hinhanh+ "&blog_cat_ids[]=4&blog_seo_title="+tieude+ "&blog_seo_keyword=&blog_seo_description=&new_blog=Create&blog_content="+noidung, "http://dichvuketoantainha.net/admin/", "");
                System.Threading.Thread.Sleep(1000);
            }
            goto loop;

            finish:

            Console.WriteLine("finish!");
            Console.ReadLine();
        }
    }
}
