/*
 Navicat Premium Data Transfer

 Source Server         : local-dev
 Source Server Type    : MongoDB
 Source Server Version : 50009
 Source Host           : localhost:27017
 Source Schema         : giftshop-demo

 Target Server Type    : MongoDB
 Target Server Version : 50009
 File Encoding         : 65001

 Date: 13/06/2022 09:56:56
*/


// ----------------------------
// Collection structure for _counters
// ----------------------------
db.getCollection("_counters").drop();
db.createCollection("_counters");

// ----------------------------
// Documents of _counters
// ----------------------------
db.getCollection("_counters").insert([ {
    _id: ObjectId("62a685866b47b6ac94eb9682"),
    collectionName: "users",
    currentCount: NumberLong("1")
} ]);
db.getCollection("_counters").insert([ {
    _id: ObjectId("62a6889d6b47b6ac94eb9795"),
    collectionName: "products",
    currentCount: NumberLong("24")
} ]);

// ----------------------------
// Collection structure for carts
// ----------------------------
db.getCollection("carts").drop();
db.createCollection("carts");

// ----------------------------
// Documents of carts
// ----------------------------
db.getCollection("carts").insert([ {
    _id: ObjectId("62a684d71e130000a90013e3")
} ]);

// ----------------------------
// Collection structure for orders
// ----------------------------
db.getCollection("orders").drop();
db.createCollection("orders");

// ----------------------------
// Documents of orders
// ----------------------------

// ----------------------------
// Collection structure for products
// ----------------------------
db.getCollection("products").drop();
db.createCollection("products");

// ----------------------------
// Documents of products
// ----------------------------
db.getCollection("products").insert([ {
    _id: ObjectId("62a6889dfe3e1f1635c61e45"),
    sku: "GS001",
    name: "Hoa khoa Miley",
    description: "Hoa khô được biết đến là một trong những lựa chọn hoàn hảo cho món quà tặng mang tính kỉ niệm dài lâu bởi khả năng trường tồn của nó theo thời gian. Điều đặc biệt ở đây là hoa khô vẫn giữ nguyên được màu sắc tự nhiên và hương thơm nhẹ nhàng. Món quà hoa khô sau khi được tặng có thể sử dụng để trang trí lâu dài trong không gian sống, bàn làm việc hay kệ sách.",
    stock: NumberInt("45"),
    price: 230000,
    detail: {
        "Màu sắc": "Tím"
    },
    traits: [
        "Sinh nhật",
        "Lưu niệm",
        "Tình yêu"
    ],
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS001",
    isActive: true,
    createdAt: ISODate("2022-06-13T00:45:17.19Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z")
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69688fe3e1f1635c61e46"),
    sku: "GS002",
    name: "Hộp Boot",
    description: "<p><strong>Hộp quà Bootleg bao gồm:</strong></p><ul><li><p>Triumph &amp; Disaster A + R Soap, Almond Milk &amp; Rosehip Oil</p></li><li><p>Whittaker's Artisan Chocolate Block, Nelson Pear &amp; Manuka Honey</p></li><li><p>Whittaker's Fruit and Nut Chocolate Bar</p></li><li><p>Seriously Good Chocolate Company Beer Jellies</p></li><li><p>Bootleg Jerky, Hot Shot.</p></li></ul>",
    stock: NumberInt("59"),
    price: 600000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Tình yêu",
        "Bạn bè"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T01:44:40.193Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS002"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69b6d193975ce7a571284"),
    sku: "GS003",
    name: "Hoa bồ công anh vĩnh cửu",
    description: "<h2>Hoa bồ công anh vĩnh cửu có gì đặc biệt?</h2><p>Hoa bồ công anh vĩnh cửu có cấu tạo được chia thành 2 phần chính: phần quả cầu thủy tinh và phần đế gỗ.</p><p>Phần quả cầu được làm từ chất liệu thủy tinh trong suốt, bên trong có chứa bông hoa bồ công anh. Điều đặc biệt ở đây đó chính là bông hoa bồ công anh này là hoa thật 100%.&nbsp; Với công nghệ chế tác tinh xảo, tỉ mỉ và sáng tạo nhà sản xuất đã lưu giữ được bông hoa bồ công anh vào trong khối pha lê, giúp bông hoa có thể tồn tại vĩnh viễn theo thời gian. Đó cũng chính là lý do tại sao lại gọi nó là hoa bồ công anh vĩnh cửu.</p>",
    stock: NumberInt("70"),
    price: 230000,
    detail: {
        color: "Trắng",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Sinh nhật",
        "Tình yêu",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:05:33.915Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS003"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69be9193975ce7a571285"),
    sku: "GS004",
    name: "Hộp nhạc gỗ trái tim",
    description: "<h2>Hộp nhạc gỗ trái tim để dành tặng cho ai và vào dịp nào</h2><p>Chiếc hộp nhạc này rất phù hợp làm quà tặng cho bạn gái. Với hình dáng trái tim biểu tượng cho tình yêu, cùng với bản nhạc du dương xua tan đi căng thẳng mệt mỏi. Đây chắc chắn sẽ là món quà lưu niệm tuyệt vời dành tặng dịp sinh nhật, <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-valentine/\">quà ngày lễ tình nhân Valentine</a>, hay các ngày lễ đặc biệt khác.</p><p>Đặc biệt hơn, bạn cũng có thể khắc những dòng thông điệp muốn nhắn nhủ tới người nhận lên trên thân chiếc hộp nhạc này. Những dòng thông điệp ý nghĩa sẽ là chất xúc tác giúp người ấy luôn nhớ tới bạn.</p>",
    stock: NumberInt("68"),
    price: 300000,
    detail: {
        color: "",
        size: "Nhỏ",
        material: "Gỗ",
        weight: "",
        dimention: ""
    },
    traits: [
        "Sinh nhật",
        "Tình yêu"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:07:37.61Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS004"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69c71193975ce7a571286"),
    sku: "GS005",
    name: "Đồng hồ cát gỗ màu nâu",
    description: "<h2>Đồng hồ cát gỗ màu nâu dùng để làm gì</h2><p>Chiếc đồng hồ cát này phù hợp làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-sinh-nhat/\">quà tặng sinh nhật</a>, noel, tết thiếu nhi,… cho bạn bè, đồng nghiệp,hay các em nhỏ.<br>Có thể là đồ trang trí với nhiều không gian trong ngôi nhà của bạn như phòng khách, phòng ngủ, phòng đọc sách,…hoặc trang trí trên bàn làm việc, bàn học, kệ tivi,…<br>Đồng hồ cát gỗ màu nâu có thể in hoặc khắc logo công ty làm quà tặng khách hàng hay các sự kiện đặc biệt.</p>",
    stock: NumberInt("80"),
    price: 180000,
    detail: {
        color: "Xanh",
        size: "Nhỏ",
        material: "Gỗ, thủy tinh",
        weight: "",
        dimention: ""
    },
    traits: [
        "Lưu niệm",
        "Sinh nhật",
        "Bạn bè"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:09:53.32Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS005"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69cec193975ce7a571287"),
    sku: "GS006",
    name: "Đồng hồ cát đồng đế gỗ",
    description: "<h2>Đồng hồ cát đồng đế gỗ dành tặng cho những ai?</h2><p>Chiếc đồng hồ cát này có thể dùng làm quà tặng trang trí trên bàn làm việc, văn phòng. Nó có thể dùng làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-sep/\">quà tặng cho sếp</a>, cho bạn đồng nghiệp trong cơ quan. Đặc biệt đối với những không gian có gam màu trầm, với nhiều đồ nội thất gỗ.</p><p>Đồng hồ cát đồng đế gỗ cũng thích hợp làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-ban-trai/\">quà tặng cho bạn trai</a>, bạn gái nhân dịp sinh nhật. Với ý nghĩa nhắn nhủ rằng quỹ thời gian của mỗi người là giới hạn. Vì vậy hãy trân trọng mỗi khoảng khắc chúng ta có và đừng lãng phí chúng.</p>",
    stock: NumberInt("89"),
    price: 500000,
    detail: {
        color: "Trắng",
        size: "Vừa",
        material: "Gỗ, thủy tinh",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Sinh nhật",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:11:56.621Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS006"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69dcb193975ce7a571288"),
    sku: "GS007",
    name: "Quả cầu tuyết True Love",
    description: "<h2>Quả cầu True love này sẽ làm quà tặng cho ai</h2><p>Quả cầu tuyết True love sẽ là món quà vô cùng ý nghĩa dành tặng cho người ấy của mình nhân dịp sinh nhật, <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-valentine/\">quà tặng valentine</a>, noel và các ngày lễ như 8/3, 20/10…Đây cũng là món quà giúp bạn tỏ tình với người thương, True love – với ý nghĩa tình yêu chân thành sẽ mang niềm vui và sự yêu thương đến với hai bạn.</p>",
    stock: NumberInt("90"),
    price: 140000,
    detail: {
        color: "Đỏ, trắng",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Tình yêu"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:15:39.955Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS007"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69e4e193975ce7a571289"),
    sku: "GS008",
    name: "Quả cầu tuyết Happy time",
    description: "<h2>Cấu tạo, tính năng và chất liệu của quả cầu tuyết Happy time</h2><p>Quả cầu tuyết Happy time có cấu tạo gồm 2 bộ phận chính. Phần chân đế phía dưới được làm từ chất liệu nhựa tổng hợp với nhiều họa tiết trang trí đẹp mắt cùng với dòng chữ Happy time. Bên trong phần chân đế là cơ cấu mạch điện giúp <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-cau-tuyet/\">quả cầu tuyết</a> này phát nhạc và sáng đèn. Phần quả cầu được làm từ thủy tinh, bên trong là dung dịch chất lỏng trong suốt. Trong quả cầu có hình ảnh đôi trai gái rất đáng yêu. Ngoài ra, bên trong dung dịch chất lỏng đó còn có các hạt tuyết nhỏ màu trắng.</p><p></p><h2>Quả cầu tuyết Happy time dành tặng cho ai</h2><p>Quả cầu tuyết này rất thích hợp làm quà tặng cho bạn gái. Bạn có thể tặng vào các dịp sinh nhật, <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-giang-sinh/\">quà tặng ngày Noel</a>, 8/3 hay Valentine, … Hình ảnh đôi nam nữ lãng mạn chắc chắn sẽ làm người đó nhớ tới bạn. Nó cũng có thể làm món đồ lưu niệm phù hợp trang trí trên bàn học, bàn làm việc.</p>",
    stock: NumberInt("56"),
    price: 130000,
    detail: {
        color: "Xanh, hồng",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Sinh nhật",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:17:50.015Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS008"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69efe193975ce7a57128a"),
    sku: "GS009",
    name: "Quả cầu tuyết cặp đôi mùa đông",
    description: "<h2>Quả cầu tuyết cặp đôi mùa đông tặng cho ai phù hợp</h2><p>Quả cầu tuyết này rất phù hợp làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-ban-trai/\">quà tặng cho bạn trai</a> hoặc quà tặng cho bạn gái. Có thể tặng vào nhiều dịp khác nhau như: sinh nhật, ngày 8/3, Valentine, quà tặng giáng sinh… Hoặc bạn có thể tặng vào bất kỳ ngày nào để tạo sự bất ngờ cho người được tặng. Với các chi tiết sắc nét và nhiều chức năng như phát nhạc, đèn Led đổi màu, phun tuyết chắc chắn người nhận sẽ rất ấn tượng với món quà này.</p>",
    stock: NumberInt("80"),
    price: 120000,
    detail: {
        color: "Trắng",
        size: "Nhỏ",
        material: "Thủy tinh",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Sinh nhật"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:20:46.288Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS009"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a69fbf193975ce7a57128b"),
    sku: "GS010",
    name: "Tượng heo ôm chữ Love",
    description: "<h3>Tượng heo chữ Love trang trí ở đâu?</h3><p>Với kích thươc nhỏ ngọn, bộ tượng heo này có thể để ở bàn làm việc, học tập hay trên xe ô tô, mỗi khi bạn buồn, nhìn thấy bộ tứ heo Love này bạn sẽ phần nào loại bỏ căng thẳng.</p><p></p><p style=\"text-align: center\"><em>Tượng heo ôm chữ Love</em></p><p>Với vẻ đẹp ngộ nghĩnh của chúng, bạn có thể sử dụng làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://iquatang.com/danh-muc/qua-tang-theo-doi-tuong/qua-tang-ban-gai/\">quà tặng bạn gái</a>, vừa tỏ tình được mà lại khiến cho nàng bật cười khi nhìn thấy vẻ dễ thương của 4 chú heo con này</p>",
    stock: NumberInt("67"),
    price: 100000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:23:59.909Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS010"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a03e193975ce7a57128c"),
    sku: "GS011",
    name: "Tranh cát 3D chuyển động",
    description: "<p>Với cơ chế hoạt động như đồng hồ cát, bức tranh cát này chuyển động thay đổi hình ảnh bên trong, các màu cát sẽ kết hợp với nhau tạo ra bức tranh khung trời, xa mạc, biển…. một cách tự nhiên. Khi bạn muốn thay đổi bước tranh thì hãy xoay chiều ngược lại, cát sẽ chảy lại từ trên xuống dưới tạo thành bức tranh khác.</p><p>Món quà sinh động này rất thích hợp làm quà tặng bạn gái, có thể giảm stress công việc, học tập, lại vừa có thể trang trí góc làm việc và học tập của mình.</p>",
    stock: NumberInt("63"),
    price: 149999,
    detail: {
        color: "Tím",
        size: "Vừa",
        material: "Gỗ, thủy tinh",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm",
        "Sinh nhật"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:26:06.704Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS011"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a088193975ce7a57128d"),
    sku: "GS012",
    name: "Đèn ngủ Led 3D",
    description: "<p>Sản phẩm sử dụng hệ thống&nbsp;đèn Led&nbsp;giúp tiết kiệm điện một cách tối ưu, nhưng vẫn đảm bảo cường độ ánh sáng dịu nhẹ, đủ thắp sáng trong căn phòng của bạn. Đèn có độ bền cao, ánh sáng không nhấp nháy gây ảnh hưởng đến giấc ngủ của bạn.</p><h2>Đèn ngủ Led 3D thích hợp để làm gì, tặng ai</h2><p>Sản phẩm này thích hợp làm quà tặng bạn gái, quà tặng cho bạn bè nhân dịp sinh nhật, 8/3, <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-8-3-20-10/\">quà tặng ngày 20/10</a> … Tùy vào mẫu đèn bạn chọn mà sẽ phù hợp cho các đối tượng khác nhau.</p>",
    stock: NumberInt("113"),
    price: 90000,
    detail: {
        color: "Trắng",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm",
        "Sinh nhật"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:27:20.129Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/SKU012"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a0dd193975ce7a57128e"),
    sku: "GS013",
    name: "Cốc sứ hình thỏ",
    description: "<h2>Cốc sứ hình thỏ dành tặng cho ai và vào dịp nào</h2><p>Chiếc cốc sứ này là một món quà rất thực tế với công năng sử dụng hàng ngày.</p><p>Bạn có thể dùng nó làm quà tặng cho bạn bè, đồng nghiệp trong công ty, làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-ban-be-dong-nghiep/\">quà tặng cho bạn cùng lớp</a> dịp sinh nhật, …</p><p>Bạn cũng có thể dùng chiếc cốc này để làm quà tặng cho bạn trai, bạn gái trong dịp sinh nhật, Giáng sinh,</p>",
    stock: NumberInt("80"),
    price: 140000,
    detail: {
        color: "Trắng",
        size: "Vừa",
        material: "Sứ",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Tình yêu"
    ],
    isActive: false,
    createdAt: ISODate("2022-06-13T02:28:45.655Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS013"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a133193975ce7a57128f"),
    sku: "GS014",
    name: "Cốc sứ Starbucks",
    description: "<h2>Cốc sứ Starbucks có gì độc đáo</h2><p>Cốc sứ Starbucks được làm từ chất liệu sứ cao cấp, tráng men bóng, dễ dàng lau chùi, với màu trắng sứ, họa tiết màu xanh tạo nên vẻ hài hòa, độc đáo. Chính giữa cốc là&nbsp;hình nữ thần Siren&nbsp;biểu tượng đáng tự hào của công ty cà phê Starbucks thành lập từ năm 1971 nhưng đến năm 1984 nó vẫn chỉ là một cái tên vô danh tại nước Mỹ. Cốc sứ Starbucks không những là một chiếc cốc uống nước tiện dụng mà còn là một sản phẩm tuyệt vời trang trí trên bàn làm việc hoặc trưng bày trong phòng khách.</p>",
    stock: NumberInt("67"),
    price: 200000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Sinh nhật",
        "Tình yêu",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:30:11.372Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS014"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a1a7193975ce7a571290"),
    sku: "GS015",
    name: "Gấu trắng ôm trái tim",
    description: "<h2>Gấu trắng ôm trái tim thích hợp dành tặng ai và vào dịp nào</h2><p>Gấu bông từ lâu đã là một trong những món quà được các em nhỏ và các bạn gái đặc biệt yêu thích. Và cũng không ngoại lệ, chú gấu trắng ôm trái tim này sẽ là món quà ý nghĩa dành tặng bạn gái, bạn cùng lớp hay các em nhỏ nhân dịp sinh nhật,&nbsp;<a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-valentine/\">quà tặng lễ tình nhân valentine</a>, 8/3, tết thiếu nhi, 20/10, giáng sinh,…Đặc biệt đây sẽ là chú gấu bông được nhiều chàng trai lựa chọn tặng bạn gái của mình với cái nhìn đầu tiên vô cùng đơn giản nhưng chú gấu bông trái tim này sẽ mang lại đến cho cô gái được sự ấm áp, vui vẻ và những giấy phút hạnh phúc khi nhận được món quà này.</p>",
    stock: NumberInt("66"),
    price: 130000,
    detail: {
        color: "Trắng, đỏ",
        size: "",
        material: "Vải bông",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Sinh nhật",
        "Gia đình",
        "Tình yêu",
        "Lưu niệm"
    ],
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS015",
    isActive: true,
    createdAt: ISODate("2022-06-13T02:32:07.102Z"),
    updateAt: ISODate("2022-06-13T02:33:39.649Z")
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a25a193975ce7a571291"),
    sku: "GS016",
    name: "Rubik 3×3",
    description: "<h2>Cấu tạo, chất liệu Rubik 3×3</h2><p>Bạn cũng đã từng thử qua trò chơi khối xoay Rubik này đúng không?&nbsp; Đây là một trong những trò chơi trí tuệ được yêu thích trên toàn Thế giới. Bất chấp mọi lứa tuổi. Mọi người đều có thể chơi nó. Từ người trưởng thành cho tới trẻ em. Có rất nhiều phiên bản rubik với độ khó tăng dần. Rubik 3×3 là một loại rubik cơ bản dành cho người mới tập chơi.</p>",
    stock: NumberInt("45"),
    price: 80000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:35:06.769Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS016"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a2d9193975ce7a571292"),
    sku: "GS017",
    name: "Hộp nhạc piano gỗ cao cấp",
    description: "<h2>Ý nghĩa của đàn PIANO</h2><p>Cuộc sống cũng giống như chiếc đàn PIANO vậy, phí trắng tượng trưng cho sự hạnh phúc, còn phím đen đại diện cho sự đau khổ, nỗi buồn. Muốn có một bản nhạc hay ta cần phải có sự kế hợp của 2 phí đó lại với nhau. Và trong cuộc sống ta nên sống chúng với những nỗi buồn đó và biến chúng thành niềm vui, niềm hạnh phúc.</p>",
    stock: NumberInt("45"),
    price: 350000,
    detail: {
        color: "",
        size: "Nhỏ",
        material: "Gỗ",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm",
        "Gia đình"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:37:13.089Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS017"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a3a7193975ce7a571293"),
    sku: "GS018",
    name: "Pha lê hoa hồng Love forever",
    description: "<h3>Thông tin chi tiết về pha lê hoa hồng Love Forever</h3><p>Bộ pha lê được chia là 2 phần: Chân đế LED và pha lê hình lập phương.</p><p>+ Chân đế: 6 x 6 x 1,5cm</p><p>+ Kích thước pha lê: 6 x 6 x 6 cm</p><p>+ Màu sắc: Thay đổi nhiều màu liên tục.</p><p>+ Nguồn điện: PIN cúc</p><p>Sản phẩm này được làm bằng chất liệu pha lê nguyên khối, sử dụng công nghệ in laser 3D khắc vào bên trong khối pha lê đó. Bởi vậy tuổi đời của những bức hình in trong đó là vĩnh cửu. Chính vì vậy, pha lê được cho là món quà có giá trị tinh thần vô cùng lớn và đặc biệt.</p>",
    stock: NumberInt("45"),
    price: 300000,
    detail: {
        color: "Tím",
        size: "",
        material: "Pha lê, thủy tinh",
        weight: "",
        dimention: ""
    },
    traits: [
        "Sinh nhật",
        "Tình yêu"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:40:39.523Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS018"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a40f193975ce7a571294"),
    sku: "GS019",
    name: "Hộp nhạc bốt điện thoại",
    description: "<h2>Cấu tạo, chất liệu và kích thước của hộp nhạc bốt điện thoại</h2><p><a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/hop-nhac/\">Hộp nhạc</a> bốt điện thoại được làm từ chất liệu gỗ tự nhiên. Các chiết tiết đều được gia công tỉ mỉ, độ hoàn thiện rất cao. Bề mặt gỗ được làm nhẵn, các đường góc cạnh cũng được làm rất sảo nét và tinh tế. Hộp nhạc có màu nâu gỗ nguyên bản. Cánh cửa có thể đóng mở. Cánh cửa này đồng thời cũng là chốt để tắt nhạc.</p>",
    stock: NumberInt("77"),
    price: 60000,
    detail: {
        color: "",
        size: "Nhỏ",
        material: "Gỗ",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Sinh nhật",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:42:23.524Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS019"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a4a7193975ce7a571295"),
    sku: "GS020",
    name: "Quả cầu tuyết ông già Noel",
    description: "<p><strong>Mỗi dịp lễ noel ( lễ giáng sinh ), ngày lễ này tại Việt Nam nói riêng và thế giới nói chung, ngày chúa sinh ra đời, vào ngày là là dịp chúng ta thể hiện sự quan tâm nhau, dành tặng nhau những món </strong><a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://iquatang.com/danh-muc/qua-tang-theo-su-kien/qua-tang-giang-sinh/\"><strong>quà tặng giáng sinh độc đáo</strong></a><strong> và ý nghĩa.</strong></p><p style=\"text-align: justify\">Tại Việt Nam và ngày này chúng ta không được nghỉ lễ như nước ngoài, chính vì thế quá trình đi tìm kiếm cửa hàng hay những nơi bán <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://iquatang.com/danh-muc/qua-tang-luu-niem/\">quà lưu niệm</a> là rất khó khăn. Chính vì thế dịch vụ Iquatang ra đời giúp các bạn tiết kiệm thời gian tìm kiếm, đi lại, hơn nữa chúng tôi luôn cập nhật những mẫu sản phẩm mới và hot nhất hiện nay</p>",
    stock: NumberInt("46"),
    price: 190000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:44:55.033Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS20"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a4f8193975ce7a571296"),
    sku: "GS021",
    name: "Mèo bông tròn",
    description: "",
    stock: NumberInt("57"),
    price: 115000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [ ],
    isActive: false,
    createdAt: ISODate("2022-06-13T02:46:16.14Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS021"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a57a193975ce7a571297"),
    sku: "GS022",
    name: "Chó bông Husky",
    description: "<h2>Chú chó bông Husky này thích hợp với những ai</h2><p>Sản phẩm này rất thích hợp làm <a target=\"_blank\" rel=\"noopener noreferrer nofollow\" href=\"https://shopquatructuyen.com/qua-tang-cho-be/\">quà tặng cho các bé</a> nhân dịp sinh nhật, tết thiếu nhi, .. Nó cũng rất thích hợp làm quà tặng cho bạn gái. Đặc biệt là những cô nàng yêu chó và thích nuôi chó. Chú chó này chắc chắn sẽ là vật thường xuyên ở trên giường của những cô nàng đó. Được ôm một chú chó bông đáng yêu này ngủ thì thật dễ chịu phải không nào.</p>",
    stock: NumberInt("78"),
    price: 240000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:48:26.088Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS022"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a5d5193975ce7a571298"),
    sku: "GS023",
    name: "Gối ôm hình đầu chó",
    description: "<h2>Kích thước, chất liệu và thiết kế của gối ôm hình đầu chó có điểm gì đặc biệt</h2><p>Gối ôm hình đầu chó đáng yêu này nằm trong bộ sưu tập gối ôm đẹp nhất được các bạn gái, trẻ em yêu thích nhất, đặc biệt là những bạn mê thú cưng. Với khuôn mặt tròn trĩnh, ngộ nghĩnh, đáng yêu, chắc chắn sẽ là món quà bạn không thể bỏ qua.</p>",
    stock: NumberInt("68"),
    price: 240000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Bạn bè",
        "Lưu niệm"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:49:57.496Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS023"
} ]);
db.getCollection("products").insert([ {
    _id: ObjectId("62a6a6e5193975ce7a571299"),
    sku: "GS024",
    name: "Hộp Chocolate Lover's",
    description: "<p>Chúng tôi biết rằng sô cô la là cuộc sống! Đó là lý do tại sao chúng tôi đã tạo ra một hộp quà hoàn toàn chứa đầy sô cô la dành cho người sành ăn.</p>",
    stock: NumberInt("56"),
    price: 500000,
    detail: {
        color: "",
        size: "",
        material: "",
        weight: "",
        dimention: ""
    },
    traits: [
        "Tình yêu",
        "Lưu niệm",
        "Sinh nhật"
    ],
    isActive: true,
    createdAt: ISODate("2022-06-13T02:54:29.408Z"),
    updateAt: ISODate("0001-01-01T00:00:00.000Z"),
    imageUrl: "https://giftshop-uit-image.s3.amazonaws.com/products/GS024"
} ]);

// ----------------------------
// Collection structure for users
// ----------------------------
db.getCollection("users").drop();
db.createCollection("users");

// ----------------------------
// Documents of users
// ----------------------------
db.getCollection("users").insert([ {
    _id: ObjectId("62a68585fe3e1f1635c61e44"),
    email: "admin@giftshop.com",
    password: "AQAAAAEAACcQAAAAEJC68ZdD86riFug0J6a9xnrB64h+0U8gIhieKSJKTBF7AlD7dZCYhHwBJrOkRnzhAg==",
    role: "ADMIN",
    lastLogin: ISODate("2022-06-13T01:40:34.097Z"),
    dateOfBirth: ISODate("0001-01-01T00:00:00.000Z"),
    isActive: true,
    createdAt: ISODate("2022-06-13T00:32:05.788Z"),
    updatedAt: ISODate("0001-01-01T00:00:00.000Z"),
    CartId: "62a684d71e130000a90013e3",
    WishlistId: "62a684d01e130000a90013e2"
} ]);

// ----------------------------
// Collection structure for verifyTokens
// ----------------------------
db.getCollection("verifyTokens").drop();
db.createCollection("verifyTokens");

// ----------------------------
// Documents of verifyTokens
// ----------------------------
db.getCollection("verifyTokens").insert([ {
    _id: ObjectId("62a685866b47b6ac94eb9685"),
    email: "admin@giftshop.com",
    token: "1d2d4448-1fcc-4855-84ad-379f7e41bd73",
    expired: ISODate("2022-06-13T00:42:06.306Z"),
    createdAt: ISODate("2022-06-13T00:32:06.306Z")
} ]);

// ----------------------------
// Collection structure for wishlists
// ----------------------------
db.getCollection("wishlists").drop();
db.createCollection("wishlists");

// ----------------------------
// Documents of wishlists
// ----------------------------
db.getCollection("wishlists").insert([ {
    _id: ObjectId("62a684d01e130000a90013e2")
} ]);
