# Hướng dẫn mở dự án: Ai Là Triệu Phú

## Mở và chạy dự án

1. Clone hoặc Pull repository `Cswinform` về máy.
2. Mở `AiLaTrieuPhu.BanGhep.slnx` bằng Visual Studio trên Windows.
3. Chọn **Build → Rebuild Solution**.
4. Nếu build thành công, nhấn **F5** để chạy.
5. Kiểm tra các chức năng từ màn hình chính, hướng dẫn, tác giả, bắt đầu chơi và thành tích.

Dự án dùng Windows Forms và đặt mục tiêu .NET `net10.0-windows`. Nếu Visual Studio báo lỗi, mở **View → Error List** để xem chi tiết.

## Cấu trúc mã nguồn

| Vị trí | Vai trò |
|---|---|
| `Program.cs` | Khởi chạy màn hình chính |
| `Forms/Main/Form1.cs` | Điều hướng, nhạc nền và thao tác màn hình chính |
| `Forms/Main/Form1.Designer.cs`, `Form1.resx` | Giao diện và tài nguyên màn hình chính |
| `Forms/Guide/HuongDanForm.cs` | Chức năng nút quay lại và mở trang tác giả |
| `Forms/Guide/HuongDanForm.Designer.cs`, `HuongDanForm.resx` | Giao diện hướng dẫn |
| `Forms/Authors/Form2.cs` | Chức năng màn hình tác giả |
| `Forms/Authors/Form2.Designer.cs`, `Form2.resx` | Giao diện và tài nguyên màn hình tác giả |
| `Forms/Game/GameForm.cs` | Hiển thị câu hỏi, trợ giúp, kết quả và lưu thành tích |
| `Forms/Game/GameForm.Designer.cs`, `GameForm.resx` | Giao diện và tài nguyên màn hình chơi |
| `Forms/Scores/ThanhTichForm.cs` | Nhập tên người chơi và xem bảng thành tích |
| `Game/GameRules.cs` | Trạng thái ván chơi, chấm đáp án và tính tiền |
| `Game/Question.cs` | Mô hình câu hỏi |
| `Game/QuestionBank.cs` | Ngân hàng câu hỏi, chủ đề và lựa chọn câu hỏi |
| `Data/Score.cs` | Mô hình thành tích và đọc/ghi file JSON |
| `Tests/TestRunner.cs` | Các kịch bản kiểm tra logic luật chơi |
| `Assets` | Nhạc nền và hình ảnh dùng trong ứng dụng |
| `Properties` | Tài nguyên dùng chung |

Các file `.Designer.cs` và `.resx` đi cùng form tương ứng. Khi chỉnh giao diện bằng WinForms Designer, giữ các file cùng thư mục và để Designer quản lý nội dung của chúng.

## Chức năng chính

- Nút **HƯỚNG DẪN** mở hướng dẫn; **QUAY LẠI** trở về màn hình chính.
- Nút **TÁC GIẢ** mở cửa sổ thông tin tác giả.
- Nút **BẮT ĐẦU** yêu cầu nhập tên trước khi mở ván chơi.
- Mỗi ván gồm 10 câu hỏi và có các quyền trợ giúp.
- Kết quả ván được ghi vào file JSON và hiển thị trên bảng thành tích.

Thành tích mặc định lưu thành `scores.json` trong thư mục chạy ứng dụng, thường là `bin/Debug/net10.0-windows/scores.json`. Thư mục đầu ra có thể bị xóa khi Clean/Rebuild hoặc xóa thủ công.

## Kiểm tra sau khi mở dự án

| Thao tác | Kết quả mong đợi |
|---|---|
| Mở ứng dụng | Màn hình chính xuất hiện |
| Mở Hướng dẫn rồi nhấn Quay lại | Trở về màn hình chính |
| Nhấn Tác giả | Cửa sổ tác giả xuất hiện |
| Bắt đầu và nhập tên | Màn hình chơi nhận tên người chơi |
| Hoàn thành một ván | Kết quả được lưu và có thể xem trong Thành tích |
| Đóng rồi mở lại ứng dụng | Thành tích còn nếu file trong thư mục đầu ra chưa bị xóa |

