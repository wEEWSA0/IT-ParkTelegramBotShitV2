using IT_ParkTelegramBotShit.DataBase.Entities;

namespace IT_ParkTelegramBotShit.DataBase.ITables;

public interface ITableCourses
{
    void CreateCourse(string courseName, string studentInviteCode, int teacherId);
    void UpdateCourseHomework(string homework, int courseId);
    void UpdateCourseNextLessonTime(DateTime nextLesson, int courseId);
    void TeacherLogOut(int teacherId);
    void UpdateTeacherName(string newName, int teacherId);
    void UpdateCourseName(string courseName, int courseId);
    void UpdateCourseInviteCode(string inviteCode, int courseId);
    bool TryGetCourseHomework(out string homework, int courseId);
    bool TryGetCourseNextLessonTime(out DateTime nextLesson, int courseId);
    bool TryGetTeacherCourses(out List<Course> courses, int teacherId);
    bool TryGetCourseByStudentInviteCode(out Course course, string inviteCode);
    bool IsCourseNameUnique(string name);
    bool IsStudentNameInCourseStudentGroupUnique(string name, int courseId);
    bool IsCourseInviteCodeUnique(string courseName);
}