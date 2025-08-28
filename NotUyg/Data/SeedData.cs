using NotUyg.Entity;

namespace NotUyg.Data
{
    public class SeedData
    {
        static public void VeriEkle( IApplicationBuilder app )
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<NotContext>();

            if (!context.Tag.Any())
            {
                context.Tag.AddRange(
                    new Entity.Tag { Name = "Eğitim" },
                    new Entity.Tag {  Name = "İş" },
                    new Entity.Tag { Name = "Gündelik" },
                     new Entity.Tag { Name = "Sağlık" }

                  );
                context.SaveChanges();
            }





        }



    }
}
