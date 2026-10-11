using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AiLaTrieuPhu
{
    public class Score
    {
        public string Id { get; set; } = "";
        public string PlayerName { get; set; } = "";
        public string Topic { get; set; } = "";
        public int CorrectCount { get; set; }
        public int Money { get; set; }
        public string Result { get; set; } = "";
        public DateTime PlayedAt { get; set; }
    }

    // Đọc/ghi tập trung tại đây để màn hình chơi và bảng thành tích dùng cùng dữ liệu.
    public class ScoreStorage
    {
        public string FilePath { get; }

        public ScoreStorage() : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "AiLaTrieuPhu", "scores.json")) { }

        public ScoreStorage(string filePath)
        {
            FilePath = Path.GetFullPath(filePath);
        }

        private bool IsValid(Score? score)
        {
            return score != null && !string.IsNullOrWhiteSpace(score.Id) &&
                !string.IsNullOrWhiteSpace(score.PlayerName) && score.PlayerName.Length <= 40 &&
                !string.IsNullOrWhiteSpace(score.Topic) &&
                score.CorrectCount >= 0 && score.CorrectCount <= 10 &&
                score.Money >= 0 && score.Money <= 25000 &&
                (score.Result == "Thắng" || score.Result == "Thua" || score.Result == "Dừng") &&
                score.PlayedAt != default;
        }

        public bool TryRead(out List<Score> scores, out string error)
        {
            scores = new List<Score>();
            error = "";
            try
            {
                // Mở trực tiếp để phân biệt chưa có file và không được quyền đọc.
                string json;
                try { json = File.ReadAllText(FilePath); }
                catch (FileNotFoundException) { return true; }
                catch (DirectoryNotFoundException) { return true; }
                List<Score>? data = JsonSerializer.Deserialize<List<Score>>(json);
                if (data == null) throw new JsonException();
                HashSet<string> ids = new HashSet<string>();
                foreach (Score score in data)
                {
                    if (!IsValid(score) || !ids.Add(score.Id)) throw new JsonException();
                }
                data.Sort(CompareScores);
                scores = data;
                return true;
            }
            catch (JsonException)
            {
                error = "File thành tích bị hỏng hoặc không đúng cấu trúc. Dữ liệu cũ được giữ nguyên.\n" + FilePath;
            }
            catch (UnauthorizedAccessException)
            {
                error = "Không có quyền đọc file thành tích.\n" + FilePath;
            }
            catch (IOException)
            {
                error = "Không đọc được file thành tích. Hãy kiểm tra file rồi thử lại.\n" + FilePath;
            }
            return false;
        }

        private static int CompareScores(Score a, Score b)
        {
            int result = b.Money.CompareTo(a.Money);
            if (result == 0) result = b.CorrectCount.CompareTo(a.CorrectCount);
            if (result == 0) result = b.PlayedAt.CompareTo(a.PlayedAt);
            return result;
        }

        public bool TrySave(Score score, out string error)
        {
            error = "";
            if (!IsValid(score))
            {
                error = "Thông tin thành tích không hợp lệ.";
                return false;
            }
            string tempPath = FilePath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
                // Ngăn hai cửa sổ chương trình cùng ghi làm mất kết quả của nhau.
                using (FileStream fileLock = new FileStream(FilePath + ".lock",
                    FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    if (!TryRead(out List<Score> scores, out error)) return false;
                    foreach (Score oldScore in scores)
                    {
                        if (oldScore.Id == score.Id) return true;
                    }
                    scores.Add(score);
                    string json = JsonSerializer.Serialize(scores,
                        new JsonSerializerOptions { WriteIndented = true });
                    // Ghi xong file tạm mới thay file chính; không xóa dữ liệu cũ trước.
                    File.WriteAllText(tempPath, json);
                    File.Move(tempPath, FilePath, true);
                }
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                error = "Không có quyền lưu thành tích.\n" + FilePath;
            }
            catch (IOException)
            {
                error = "Chưa lưu được thành tích. File có thể đang được sử dụng hoặc ổ đĩa không ghi được.\n" + FilePath;
            }
            finally
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
            return false;
        }
    }
}
