public class User
{
    public string Id { get; set; } // เปลี่ยนจาก int เป็น string
    public string Name { get; set; }
    public string Email { get; set; } // เพิ่ม Email
    public int Age { get; set; } // เพิ่ม Age

    // Constructor ที่กำหนดค่าเริ่มต้นให้กับ Name
    public User()
    {
        Id = "";
        Name = "";
        Email = ""; // กำหนดค่าเริ่มต้นให้ Email
        Age = 0; // กำหนดค่าเริ่มต้นให้ Age
    }
}