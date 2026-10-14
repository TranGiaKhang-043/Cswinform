# Ai Là Triệu Phú

Ứng dụng trò chơi hỏi đáp được xây dựng bằng C# và Windows Forms.

## Bắt đầu

1. Mở `AiLaTrieuPhu.BanGhep.slnx` bằng Visual Studio trên Windows.
2. Chọn **Build → Rebuild Solution**.
3. Nhấn **F5** để chạy ứng dụng.

Xem [hướng dẫn mở và sử dụng dự án](docs/HUONG_DAN_MO_DU_AN.md) để biết thêm về chức năng và cách kiểm tra.

## Cấu trúc thư mục

| Thư mục hoặc tệp | Nội dung |
|---|---|
| `Program.cs` | Điểm khởi chạy ứng dụng |
| `Forms/Main` | Màn hình chính và điều hướng |
| `Forms/Guide` | Màn hình hướng dẫn |
| `Forms/Authors` | Màn hình thông tin tác giả |
| `Forms/Game` | Giao diện và tương tác trong ván chơi |
| `Forms/Scores` | Nhập tên người chơi và bảng thành tích |
| `Game` | Luật chơi, mô hình câu hỏi và ngân hàng câu hỏi |
| `Data` | Mô hình thành tích và chức năng đọc/ghi điểm |
| `Tests` | Các kịch bản kiểm tra logic luật chơi |
| `Assets` | Nhạc nền và hình ảnh |
| `Properties` | Tài nguyên dùng chung của dự án |
| `docs` | Tài liệu hướng dẫn |

## Luồng hoạt động chính

`Program.cs` mở màn hình chính trong `Forms/Main`. Khi bắt đầu, màn hình chơi trong `Forms/Game` sử dụng luật ở `Game` và lưu kết quả thông qua `Data/Score.cs`. Các màn hình hướng dẫn, tác giả và thành tích nằm riêng trong `Forms`.
