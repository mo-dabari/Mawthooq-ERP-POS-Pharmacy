namespace Mawthooq.Domain.Products.Enums
{
    public enum PackagingTemplate
    {
        // 1. Box -> Strips -> Tablets/Capsules (e.g., Panadol, Cataflam)
        BoxWithStripsAndUnits = 1,

        // 2. Box -> Direct Units like Ampoules or Sachets (e.g., Injections, Fervent)
        BoxWithDirectUnits = 2,

        // 3. Single Unit / Piece Only (e.g., Syrups, Creams, Sprays, Devices)
        SingleUnit = 3,

        // 4. Outer Box/Carton -> Inner Boxes -> Pieces (e.g., Diapers, Syringes)
        OuterBoxWithInnerBoxesAndPieces = 4
    }
}
