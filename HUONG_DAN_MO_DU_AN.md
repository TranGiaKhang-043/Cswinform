# Dự án WinForms: Ai Là Triệu Phú

## Mở và chạy dự án

1. Clone hoặc Pull repository `Cswinform` về máy.
2. Mở `AiLaTrieuPhu.BanGhep.slnx` bằng Visual Studio.
3. Chọn **Build → Rebuild Solution**.
4. Nếu build thành công, nhấn **F5** để chạy.
5. Kiểm tra lần lượt nút Hướng dẫn, nút âm thanh, Bắt đầu, Thành tích và các hành động trong màn hình chơi.

Nếu Visual Studio báo lỗi, mở **View → Error List** và xử lý lỗi trước khi chạy; không chạy bản build cũ sau khi build thất bại.

## Chức năng hiện có

- Màn hình chính giữ hình nền, tiêu đề và bố cục đã thiết kế bằng WinForms Designer.
- Nút **HƯỚNG DẪN** mở `HuongDanForm`; nút **QUAY LẠI** đóng form hướng dẫn để trở lại màn hình chính.
- Nhạc nền đọc từ `Assets/Backgroundmusic.mp3`, phát lặp; nút loa bật/tắt tiếng nhạc.
- Nút **BẮT ĐẦU** yêu cầu nhập tên người chơi trước khi mở màn hình game.
- Màn hình chơi có bốn chủ đề CNTT, mỗi ván 10 câu và các quyền trợ giúp 50:50, hỏi khán giả, gọi điện thoại.
- Kết thúc ván bằng thắng, thua hoặc dừng sẽ ghi thành tích gồm tên người chơi, chủ đề, số câu đúng, tiền thực nhận, kết quả và thời gian.
- Nút **THÀNH TÍCH** mở bảng xếp hạng, có thể tải lại danh sách.
- Dữ liệu thành tích mặc định lưu tại thư mục dữ liệu cục bộ của người dùng Windows: `%LOCALAPPDATA%/AiLaTrieuPhu/scores.json`. Thành tích không đồng bộ tự động giữa các máy tính.
- Khi đóng cửa sổ chơi trong lúc ván còn đang diễn ra, chương trình hỏi xác nhận; để ghi nhận tiền đã đạt được, hãy chọn Không rồi dùng nút Dừng cuộc chơi.

## Các tệp chính

| Tệp | Vai trò |
|---|---|
| `Program.cs` | Khởi chạy màn hình chính |
| `Form1.cs` | Logic màn hình chính, nhạc nền, điều hướng và nhập tên |
| `Form1.Designer.cs`, `Form1.resx` | Giao diện và tài nguyên màn hình chính |
| `HuongDanForm.cs` | Xử lý nút Quay lại của trang hướng dẫn |
| `HuongDanForm.Designer.cs`, `HuongDanForm.resx` | Giao diện trang hướng dẫn |
| `GameForm.cs` | Tương tác màn hình chơi, xử lý trợ giúp, kết quả và lưu thành tích |
| `GameForm.Designer.cs`, `GameForm.resx` | Giao diện và tài nguyên màn hình chơi |
| `Score.cs` | Mô hình điểm và đọc/ghi dữ liệu thành tích |
| `ThanhTichForm.cs` | Form nhập tên và form bảng thành tích |
| `GameRules.cs` | Trạng thái ván, chấm đáp án, chuyển câu và tính tiền |
| `Question.cs` | Mô hình câu hỏi |
| `QuestionBank.cs` | Ngân hàng câu hỏi, chủ đề và trộn câu/đáp án |
| `TestRunner.cs` | Kịch bản kiểm tra logic luật chơi |
| `Assets/Backgroundmusic.mp3` | Nhạc nền được sao chép sang thư mục đầu ra khi build |

## Checklist kiểm thử sau khi Pull

| Thao tác | Kết quả cần thấy |
|---|---|
| Mở ứng dụng | Màn hình chính xuất hiện và nhạc nền bắt đầu phát |
| Nhấn nút loa | Nhạc tắt tiếng/bật tiếng và biểu tượng đổi tương ứng |
| Nhấn Hướng dẫn | Trang hướng dẫn mở; nhấn Quay lại thì trở về màn hình chính |
| Nhấn Bắt đầu, nhập tên | Màn hình game mở và nhận tên vừa nhập |
| Nhấn Hủy ở hộp nhập tên | Không mở màn hình game |
| Hoàn thành một ván bằng thắng/thua/dừng | Thành tích được lưu một lần |
| Mở Thành tích | Bảng hiển thị các lượt chơi đã lưu |
| Đóng/mở lại ứng dụng | Thành tích trước đó còn trong thư mục LocalAppData của cùng người dùng Windows |
| Đóng game khi đang chơi | Có hộp thoại xác nhận thoát |
| Chơi lại hoặc quay về màn hình chính | Trạng thái game được làm mới đúng và không mở chồng form |

**Ghi chú kiểm thử:** Mã nguồn đã được rà soát tĩnh sau khi tích hợp, nhưng build và thao tác UI thực tế vẫn cần được xác nhận trong Visual Studio trên máy Windows. Báo cáo lỗi trong Error List nếu Rebuild Solution chưa thành công.
