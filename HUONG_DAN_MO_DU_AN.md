# Bản ghép màn hình chính và màn hình chơi

## Việc Hào cần làm đầu tiên

1. Giải nén toàn bộ tệp ZIP vào một thư mục mới, không chép đè dự án cũ.
2. Trong thư mục vừa giải nén, mở `AiLaTrieuPhu.BanGhep.slnx` bằng Visual Studio.
3. Nhấn **Ctrl+Shift+B** để biên dịch: kiểm tra xem mã có tạo được chương trình không.
4. Khi không có lỗi, nhấn **F5** để chạy.
5. Bấm **BẮT ĐẦU** trên màn hình chính. Màn hình chơi phải hiện câu 1/10 và tiền $0.

Nếu Visual Studio báo lỗi, mở **View → Error List** (danh sách lỗi), chụp các dòng lỗi gửi lại. Không chọn chạy bản cũ nếu Visual Studio đề nghị sau khi biên dịch thất bại.

## Những gì đã ghép

- Giữ hình nền, tiêu đề và bố cục màn hình chính của Quân.
- Nút Bắt đầu mở màn hình chơi và tạm ẩn màn hình chính.
- Đóng màn hình chơi bằng dấu X hoặc Alt+F4 sẽ trở lại màn hình chính.
- Nút Thoát trên màn hình chính đóng ứng dụng.
- Màn hình chơi có bốn chủ đề công nghệ thông tin. Bấm ô Chủ đề trước khi trả lời hoặc dùng trợ giúp ở câu đầu để đổi chủ đề.
- Mỗi ván có 10 câu, tiền USD và mốc an toàn theo thỏa thuận của nhóm.
- Bấm Chơi lại sau khi kết thúc sẽ tạo ván mới và đặt lại quyền trợ giúp.
- Khi trở lại màn hình chính rồi bấm Bắt đầu, ứng dụng tạo ván mới trong chủ đề vừa chơi.
- Bộ luật của mỗi chủ đề được giữ trong lần mở ứng dụng, giúp ván sau tránh các câu của ván liền trước trong cùng chủ đề. Đóng hẳn ứng dụng sẽ xóa lịch sử tạm này. Ngân hàng hữu hạn không thể đảm bảo không lặp mãi mãi.

## Phần còn chờ thành viên khác

- Ba nút Hướng dẫn, Âm thanh và Thành tích trên màn hình chính đang tạm khóa vì chưa có mã của Khang và Hoàng để nối vào.
- Các trợ giúp 50:50, khán giả và gọi điện trong màn hình chơi được giữ từ bản Hào đã làm cùng trợ lý. Đây là phần có thể bàn giao cho Khang rà soát và tích hợp với phần của bạn ấy.
- Bản ghép chưa thêm âm thanh, hướng dẫn chơi hoặc lưu thành tích. Không coi đây là bản hoàn thành toàn bộ đồ án.

## Tìm tệp theo trách nhiệm

| Tệp | Vai trò |
|---|---|
| `Program.cs` | Mở màn hình chính khi chạy ứng dụng |
| `Form1.cs`, `Form1.Designer.cs`, `Form1.resx` | Bộ ba tệp màn hình chính của Quân; lớp bên trong tên `frmMain` |
| `GameForm.cs`, `GameForm.Designer.cs`, `GameForm.resx` | Bộ ba tệp màn hình chơi đã ghép; đổi tên từ Form1 của Hào để tránh chép đè |
| `Question.cs` | Mô tả một câu hỏi |
| `QuestionBank.cs` | Ngân hàng câu hỏi theo bốn chủ đề và trộn câu, đáp án |
| `GameRules.cs` | Trạng thái ván, chấm đáp án, chuyển câu và tính tiền |
| `TestRunner.cs` | Bảy kịch bản kiểm tra luật chơi, không phụ thuộc giao diện |

Bốn tệp xử lý được giữ nguyên nội dung từ các tệp Hào đã cung cấp. `Question .cs` được đặt tên gọn thành `Question.cs`; `QuestionBank1.cs` được đặt thành `QuestionBank.cs`. Không thêm lại bản tên cũ vào cùng dự án vì sẽ bị khai báo lớp trùng.

## Các thao tác kiểm tra tiếp theo

Bản ghép đã biên dịch thành công bằng bộ công cụ Visual Studio đang có trên máy: **0 lỗi, 11 cảnh báo**. Có 9 cảnh báo về dữ liệu có thể thiếu giá trị trong các tệp xử lý cũ và 2 cảnh báo từ thư viện Windows Media Player vốn có trong dự án của Quân. Hai tệp màn hình đã ghép không phát sinh cảnh báo biên dịch. Tài nguyên hình ảnh của cả hai cửa sổ đã được đóng kèm đúng tên.

| Thao tác | Kết quả cần thấy |
|---|---|
| Bắt đầu từ màn hình chính | Vào câu 1/10, tiền $0 |
| Mở danh sách chủ đề | Có bốn chủ đề; chọn một chủ đề thì câu hỏi thuộc chủ đề đó |
| Bấm Dừng ngay ở câu 1 rồi xác nhận | Kết thúc với $0 và có nút Chơi lại |
| Bấm Chơi lại | Về câu 1/10, tiền $0, trợ giúp được khôi phục |
| Đóng cửa sổ chơi | Màn hình chính hiện lại |
| Bấm Bắt đầu lần nữa | Ván mới, không mở chồng nhiều cửa sổ chơi |
| Bấm Thoát trên màn hình chính | Ứng dụng đóng |

Biên dịch thành công không thay cho kiểm tra thao tác trực tiếp. Lần trước Windows đã chặn chương trình kiểm tra tự động bằng chính sách kiểm soát ứng dụng, nên chưa xác nhận các thao tác giao diện tự động trong bản ghép này. Hào thực hiện phép thử đầu tiên ở trên trong Visual Studio rồi báo kết quả.
