﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nursan.Domain.TORKS;

public partial class Nrsclsdeg
{
    [ForeignKey("Id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]

    public int Id { get; set; }
    public string? Nr { get; set; }

    public string? ReceteName { get; set; }

    public short? ReceteNo { get; set; }

    public string? Kacstep { get; set; }

    public string? Tork1 { get; set; }

    public string? Aci1 { get; set; }

    public string? Hata1 { get; set; }

    public string? Tabanca1 { get; set; }

    public string? Config1 { get; set; }

    public string? Kutu1 { get; set; }

    public string? Vida1 { get; set; }

    public string? Tork2 { get; set; }

    public string? Aci2 { get; set; }

    public string? Hata2 { get; set; }

    public string? Tabanca2 { get; set; }

    public string? Config2 { get; set; }

    public string? Kutu2 { get; set; }

    public string? Vida2 { get; set; }

    public string? Tork3 { get; set; }

    public string? Aci3 { get; set; }

    public string? Hata3 { get; set; }

    public string? Tabanca3 { get; set; }

    public string? Config3 { get; set; }

    public string? Kutu3 { get; set; }

    public string? Vida3 { get; set; }

    public string? Tork4 { get; set; }

    public string? Aci4 { get; set; }

    public string? Hata4 { get; set; }

    public string? Tabanca4 { get; set; }

    public string? Config4 { get; set; }

    public string? Kutu4 { get; set; }

    public string? Vida4 { get; set; }

    public string? Tork5 { get; set; }

    public string? Aci5 { get; set; }

    public string? Hata5 { get; set; }

    public string? Tabanca5 { get; set; }

    public string? Config5 { get; set; }

    public string? Kutu5 { get; set; }

    public string? Vida5 { get; set; }

    public string? Tork6 { get; set; }

    public string? Aci6 { get; set; }

    public string? Hata6 { get; set; }

    public string? Tabanca6 { get; set; }

    public string? Config6 { get; set; }

    public string? Kutu6 { get; set; }

    public string? Vida6 { get; set; }

    public string? Tork7 { get; set; }

    public string? Aci7 { get; set; }

    public string? Hata7 { get; set; }

    public string? Tabanca7 { get; set; }

    public string? Config7 { get; set; }

    public string? Kutu7 { get; set; }

    public string? Vida7 { get; set; }

    public string? Tork8 { get; set; }

    public string? Aci8 { get; set; }

    public string? Hata8 { get; set; }

    public string? Tabanca8 { get; set; }

    public string? Config8 { get; set; }

    public string? Kutu8 { get; set; }

    public string? Vida8 { get; set; }

    public string? Tork9 { get; set; }

    public string? Aci9 { get; set; }

    public string? Hata9 { get; set; }

    public string? Tabanca9 { get; set; }

    public string? Config9 { get; set; }

    public string? Kutu9 { get; set; }

    public string? Vida9 { get; set; }

    public string? Tork10 { get; set; }

    public string? Aci10 { get; set; }

    public string? Hata10 { get; set; }

    public string? Tabanca10 { get; set; }

    public string? Config10 { get; set; }

    public string? Kutu10 { get; set; }

    public string? Vida10 { get; set; }

    public string? Tork11 { get; set; }

    public string? Aci11 { get; set; }

    public string? Hata11 { get; set; }

    public string? Tabanca11 { get; set; }

    public string? Config11 { get; set; }

    public string? Kutu11 { get; set; }

    public string? Vida11 { get; set; }

    public string? Tork12 { get; set; }

    public string? Aci12 { get; set; }

    public string? Hata12 { get; set; }

    public string? Tabanca12 { get; set; }

    public string? Config12 { get; set; }

    public string? Kutu12 { get; set; }

    public string? Vida12 { get; set; }

    public string? Tork13 { get; set; }

    public string? Aci13 { get; set; }

    public string? Hata13 { get; set; }

    public string? Tabanca13 { get; set; }

    public string? Config13 { get; set; }

    public string? Kutu13 { get; set; }

    public string? Vida13 { get; set; }

    public string? Tork14 { get; set; }

    public string? Aci14 { get; set; }

    public string? Hata14 { get; set; }

    public string? Tabanca14 { get; set; }

    public string? Config14 { get; set; }

    public string? Kutu14 { get; set; }

    public string? Vida14 { get; set; }

    public string? Tork15 { get; set; }

    public string? Aci15 { get; set; }

    public string? Hata15 { get; set; }

    public string? Tabanca15 { get; set; }

    public string? Config15 { get; set; }

    public string? Kutu15 { get; set; }

    public string? Vida15 { get; set; }

    public string? Tork16 { get; set; }

    public string? Aci16 { get; set; }

    public string? Hata16 { get; set; }

    public string? Tabanca16 { get; set; }

    public string? Config16 { get; set; }

    public string? Kutu16 { get; set; }

    public string? Vida16 { get; set; }

    public string? Tork17 { get; set; }

    public string? Aci17 { get; set; }

    public string? Hata17 { get; set; }

    public string? Tabanca17 { get; set; }

    public string? Config17 { get; set; }

    public string? Kutu17 { get; set; }

    public string? Vida17 { get; set; }

    public string? Tork18 { get; set; }

    public string? Aci18 { get; set; }

    public string? Hata18 { get; set; }

    public string? Tabanca18 { get; set; }

    public string? Config18 { get; set; }

    public string? Kutu18 { get; set; }

    public string? Vida18 { get; set; }

    public string? Tork19 { get; set; }

    public string? Aci19 { get; set; }

    public string? Hata19 { get; set; }

    public string? Tabanca19 { get; set; }

    public string? Config19 { get; set; }

    public string? Kutu19 { get; set; }

    public string? Vida19 { get; set; }

    public string? Tork20 { get; set; }

    public string? Aci20 { get; set; }

    public string? Hata20 { get; set; }

    public string? Tabanca20 { get; set; }

    public string? Config20 { get; set; }

    public string? Kutu20 { get; set; }

    public string? Vida20 { get; set; }

    public string? Tork21 { get; set; }

    public string? Aci21 { get; set; }

    public string? Hata21 { get; set; }

    public string? Tabanca21 { get; set; }

    public string? Config21 { get; set; }

    public string? Kutu21 { get; set; }

    public string? Vida21 { get; set; }

    public string? Tork22 { get; set; }

    public string? Aci22 { get; set; }

    public string? Hata22 { get; set; }

    public string? Tabanca22 { get; set; }

    public string? Config22 { get; set; }

    public string? Kutu22 { get; set; }

    public string? Vida22 { get; set; }

    public string? Tork23 { get; set; }

    public string? Aci23 { get; set; }

    public string? Hata23 { get; set; }

    public string? Tabanca23 { get; set; }

    public string? Config23 { get; set; }

    public string? Kutu23 { get; set; }

    public string? Vida23 { get; set; }

    public string? Tork24 { get; set; }

    public string? Aci24 { get; set; }

    public string? Hata24 { get; set; }

    public string? Tabanca24 { get; set; }

    public string? Config24 { get; set; }

    public string? Kutu24 { get; set; }

    public string? Vida24 { get; set; }

    public string? Tork25 { get; set; }

    public string? Aci25 { get; set; }

    public string? Hata25 { get; set; }

    public string? Tabanca25 { get; set; }

    public string? Config25 { get; set; }

    public string? Kutu25 { get; set; }

    public string? Vida25 { get; set; }

    public string? Tork26 { get; set; }

    public string? Aci26 { get; set; }

    public string? Hata26 { get; set; }

    public string? Tabanca26 { get; set; }

    public string? Config26 { get; set; }

    public string? Kutu26 { get; set; }

    public string? Vida26 { get; set; }

    public string? Tork27 { get; set; }

    public string? Aci27 { get; set; }

    public string? Hata27 { get; set; }

    public string? Tabanca27 { get; set; }

    public string? Config27 { get; set; }

    public string? Kutu27 { get; set; }

    public string? Vida27 { get; set; }

    public string? Tork28 { get; set; }

    public string? Aci28 { get; set; }

    public string? Hata28 { get; set; }

    public string? Tabanca28 { get; set; }

    public string? Config28 { get; set; }

    public string? Kutu28 { get; set; }

    public string? Vida28 { get; set; }

    public string? Tork29 { get; set; }

    public string? Aci29 { get; set; }

    public string? Hata29 { get; set; }

    public string? Tabanca29 { get; set; }

    public string? Config29 { get; set; }

    public string? Kutu29 { get; set; }

    public string? Vida29 { get; set; }

    public string? Tork30 { get; set; }

    public string? Aci30 { get; set; }

    public string? Hata30 { get; set; }

    public string? Tabanca30 { get; set; }

    public string? Config30 { get; set; }

    public string? Kutu30 { get; set; }

    public string? Vida30 { get; set; }

    public string? Tork31 { get; set; }

    public string? Aci31 { get; set; }

    public string? Hata31 { get; set; }

    public string? Tabanca31 { get; set; }

    public string? Config31 { get; set; }

    public string? Kutu31 { get; set; }

    public string? Vida31 { get; set; }

    public string? Tork32 { get; set; }

    public string? Aci32 { get; set; }

    public string? Hata32 { get; set; }

    public string? Tabanca32 { get; set; }

    public string? Config32 { get; set; }

    public string? Kutu32 { get; set; }

    public string? Vida32 { get; set; }

    public string? Tork33 { get; set; }

    public string? Aci33 { get; set; }

    public string? Hata33 { get; set; }

    public string? Tabanca33 { get; set; }

    public string? Config33 { get; set; }

    public string? Kutu33 { get; set; }

    public string? Vida33 { get; set; }

    public string? Tork34 { get; set; }

    public string? Aci34 { get; set; }

    public string? Hata34 { get; set; }

    public string? Tabanca34 { get; set; }

    public string? Config34 { get; set; }

    public string? Kutu34 { get; set; }

    public string? Vida34 { get; set; }

    public string? Tork35 { get; set; }

    public string? Aci35 { get; set; }

    public string? Hata35 { get; set; }

    public string? Tabanca35 { get; set; }

    public string? Config35 { get; set; }

    public string? Kutu35 { get; set; }

    public string? Vida35 { get; set; }

    public string? Tork36 { get; set; }

    public string? Aci36 { get; set; }

    public string? Hata36 { get; set; }

    public string? Tabanca36 { get; set; }

    public string? Config36 { get; set; }

    public string? Kutu36 { get; set; }

    public string? Vida36 { get; set; }

    public string? Tork37 { get; set; }

    public string? Aci37 { get; set; }

    public string? Hata37 { get; set; }

    public string? Tabanca37 { get; set; }

    public string? Config37 { get; set; }

    public string? Kutu37 { get; set; }

    public string? Vida37 { get; set; }

    public string? Tork38 { get; set; }

    public string? Aci38 { get; set; }

    public string? Hata38 { get; set; }

    public string? Tabanca38 { get; set; }

    public string? Config38 { get; set; }

    public string? Kutu38 { get; set; }

    public string? Vida38 { get; set; }

    public string? Tork39 { get; set; }

    public string? Aci39 { get; set; }

    public string? Hata39 { get; set; }

    public string? Tabanca39 { get; set; }

    public string? Config39 { get; set; }

    public string? Kutu39 { get; set; }

    public string? Vida39 { get; set; }

    public string? Tork40 { get; set; }

    public string? Aci40 { get; set; }

    public string? Hata40 { get; set; }

    public string? Tabanca40 { get; set; }

    public string? Config40 { get; set; }

    public string? Kutu40 { get; set; }

    public string? Vida40 { get; set; }

    public string? Tork41 { get; set; }

    public string? Aci41 { get; set; }

    public string? Hata41 { get; set; }

    public string? Tabanca41 { get; set; }

    public string? Config41 { get; set; }

    public string? Kutu41 { get; set; }

    public string? Vida41 { get; set; }

    public string? Tork42 { get; set; }

    public string? Aci42 { get; set; }

    public string? Hata42 { get; set; }

    public string? Tabanca42 { get; set; }

    public string? Config42 { get; set; }

    public string? Kutu42 { get; set; }

    public string? Vida42 { get; set; }

    public string? Tork43 { get; set; }

    public string? Aci43 { get; set; }

    public string? Hata43 { get; set; }

    public string? Tabanca43 { get; set; }

    public string? Config43 { get; set; }

    public string? Kutu43 { get; set; }

    public string? Vida43 { get; set; }

    public string? Tork44 { get; set; }

    public string? Aci44 { get; set; }

    public string? Hata44 { get; set; }

    public string? Tabanca44 { get; set; }

    public string? Config44 { get; set; }

    public string? Kutu44 { get; set; }

    public string? Vida44 { get; set; }

    public string? Tork45 { get; set; }

    public string? Aci45 { get; set; }

    public string? Hata45 { get; set; }

    public string? Tabanca45 { get; set; }

    public string? Config45 { get; set; }

    public string? Kutu45 { get; set; }

    public string? Vida45 { get; set; }

    public string? Tork46 { get; set; }

    public string? Aci46 { get; set; }

    public string? Hata46 { get; set; }

    public string? Tabanca46 { get; set; }

    public string? Config46 { get; set; }

    public string? Kutu46 { get; set; }

    public string? Vida46 { get; set; }

    public string? Tork47 { get; set; }

    public string? Aci47 { get; set; }

    public string? Hata47 { get; set; }

    public string? Tabanca47 { get; set; }

    public string? Config47 { get; set; }

    public string? Kutu47 { get; set; }

    public string? Vida47 { get; set; }

    public string? Tork48 { get; set; }

    public string? Aci48 { get; set; }

    public string? Hata48 { get; set; }

    public string? Tabanca48 { get; set; }

    public string? Config48 { get; set; }

    public string? Kutu48 { get; set; }

    public string? Vida48 { get; set; }

    public string? Tork49 { get; set; }

    public string? Aci49 { get; set; }

    public string? Hata49 { get; set; }

    public string? Tabanca49 { get; set; }

    public string? Config49 { get; set; }

    public string? Kutu49 { get; set; }

    public string? Vida49 { get; set; }

    public string? Tork50 { get; set; }

    public string? Aci50 { get; set; }

    public string? Hata50 { get; set; }

    public string? Tabanca50 { get; set; }

    public string? Config50 { get; set; }

    public string? Kutu50 { get; set; }

    public string? Vida50 { get; set; }

    public string? Calisan { get; set; }
}
