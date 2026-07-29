import 'package:flutter/material.dart';

void main() {
  runApp(const RestaurantHRApp());
}

class RestaurantHRApp extends StatelessWidget {
  const RestaurantHRApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'إدارة الموارد البشرية 2026',
      theme: ThemeData.dark().copyWith(
        scaffoldBackgroundColor: const Color(0xFF0F172A),
        primaryColor: const Color(0xFF2563EB),
        cardColor: const Color(0xFF1E293B),
      ),
      builder: (context, child) {
        return Directionality(
          textDirection: TextDirection.rtl, // دعم كامل للغة العربية
          child: child!,
        );
      },
      home: const DashboardScreen(),
    );
  }
}

class DashboardScreen extends StatefulWidget {
  const DashboardScreen({super.key});

  @override
  State<DashboardScreen> createState() => _DashboardScreenState();
}

class _DashboardScreenState extends State<DashboardScreen> {
  // بيانات وهمية قابلة للتعديل والبحث
  final List<Map<String, dynamic>> _allAttendances = [
    {"date": "2026-07-30", "name": "أحمد محمود", "checkIn": "09:00", "checkOut": "17:00", "overtime": "1.0"},
    {"date": "2026-07-30", "name": "سارة خالد", "checkIn": "09:15", "checkOut": "17:00", "overtime": "0.0"},
    {"date": "2026-07-29", "name": "رامي سعيد", "checkIn": "08:50", "checkOut": "16:00", "overtime": "0.5"},
  ];

  List<Map<String, dynamic>> _filteredAttendances = [];
  String _searchQuery = "";

  @override
  void initState() {
    super.initState();
    _filteredAttendances = _allAttendances; // عرض الكل في البداية
  }

  // دالة البحث اللحظي
  void _filterData(String query) {
    setState(() {
      _searchQuery = query;
      _filteredAttendances = _allAttendances
          .where((item) => item["name"].toString().contains(query))
          .toList();
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Row(
        children: [
          // 1. الشريط الجانبي (Sidebar)
          Container(
            width: 250,
            color: const Color(0xFF1E293B),
            padding: const EdgeInsets.all(20),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Row(
                  children: [
                    Container(
                      padding: const EdgeInsets.all(10),
                      decoration: BoxDecoration(
                        color: Colors.blueAccent.withOpacity(0.2),
                        borderRadius: BorderRadius.circular(10),
                      ),
                      child: const Icon(Icons.restaurant, color: Colors.blueAccent),
                    ),
                    const SizedBox(width: 10),
                    const Text("شؤون المطعم", style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
                  ],
                ),
                const SizedBox(height: 40),
                _buildNavItem(Icons.dashboard, "لوحة التحكم", true),
                _buildNavItem(Icons.people, "دليل الموظفين", false),
                _buildNavItem(Icons.access_time, "سجل الدوام", false),
                _buildNavItem(Icons.attach_money, "الرواتب والسلف", false),
              ],
            ),
          ),

          // 2. المحتوى الرئيسي
          Expanded(
            child: Padding(
              padding: const EdgeInsets.all(30.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // الهيدر
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      const Text("مرحباً بك مجدداً 👋", style: TextStyle(fontSize: 28, fontWeight: FontWeight.bold)),
                      ElevatedButton.icon(
                        onPressed: () {
                          // هنا نربط نافذة الإضافة مستقبلاً
                          ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('سيتم فتح نافذة الإضافة!')));
                        },
                        style: ElevatedButton.styleFrom(
                          backgroundColor: const Color(0xFF2563EB),
                          padding: const EdgeInsets.symmetric(horizontal: 20, vertical: 15),
                          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
                        ),
                        icon: const Icon(Icons.add, color: Colors.white),
                        label: const Text("تسجيل حضور يدوي", style: TextStyle(color: Colors.white, fontWeight: FontWeight.bold)),
                      )
                    ],
                  ),
                  const SizedBox(height: 30),

                  // البطاقات الإحصائية
                  Row(
                    children: [
                      _buildStatCard("🏆", "نجم الورديات", "أحمد محمود (شيف)", Colors.amber),
                      const SizedBox(width: 20),
                      _buildStatCard("👥", "الحضور اليوم", "12 / 15 موظف", Colors.blue),
                      const SizedBox(width: 20),
                      _buildStatCard("⚡", "مؤشر الانضباط", "94% ممتاز", Colors.green),
                    ],
                  ),
                  const SizedBox(height: 30),

                  // شريط البحث
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 15),
                    decoration: BoxDecoration(
                      color: const Color(0xFF1E293B),
                      borderRadius: BorderRadius.circular(12),
                    ),
                    child: TextField(
                      onChanged: _filterData,
                      decoration: const InputDecoration(
                        icon: Icon(Icons.search, color: Colors.grey),
                        hintText: "ابحث عن اسم الموظف هنا...",
                        hintStyle: TextStyle(color: Colors.grey),
                        border: InputBorder.none,
                      ),
                    ),
                  ),
                  const SizedBox(height: 20),

                  // جدول البيانات
                  Expanded(
                    child: Container(
                      width: double.infinity,
                      decoration: BoxDecoration(
                        color: const Color(0xFF1E293B),
                        borderRadius: BorderRadius.circular(12),
                      ),
                      child: SingleChildScrollView(
                        child: DataTable(
                          columns: const [
                            DataColumn(label: Text("التاريخ", style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey))),
                            DataColumn(label: Text("اسم الموظف", style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey))),
                            DataColumn(label: Text("وقت الدخول", style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey))),
                            DataColumn(label: Text("وقت الخروج", style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey))),
                            DataColumn(label: Text("إضافي", style: TextStyle(fontWeight: FontWeight.bold, color: Colors.grey))),
                          ],
                          rows: _filteredAttendances.map((item) {
                            return DataRow(cells: [
                              DataCell(Text(item["date"]!)),
                              DataCell(Text(item["name"]!, style: const TextStyle(fontWeight: FontWeight.bold))),
                              DataCell(Text(item["checkIn"]!)),
                              DataCell(Text(item["checkOut"]!)),
                              DataCell(Text(item["overtime"]!)),
                            ]);
                          }).toList(),
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  // عنصر القائمة الجانبية
  Widget _buildNavItem(IconData icon, String title, bool isActive) {
    return Container(
      margin: const EdgeInsets.only(bottom: 15),
      padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 15),
      decoration: BoxDecoration(
        color: isActive ? Colors.blueAccent.withOpacity(0.1) : Colors.transparent,
        borderRadius: BorderRadius.circular(10),
      ),
      child: Row(
        children: [
          Icon(icon, color: isActive ? Colors.blueAccent : Colors.grey, size: 22),
          const SizedBox(width: 15),
          Text(title, style: TextStyle(color: isActive ? Colors.white : Colors.grey, fontSize: 16)),
        ],
      ),
    );
  }

  // البطاقة الإحصائية
  Widget _buildStatCard(String emoji, String title, String value, Color color) {
    return Expanded(
      child: Container(
        padding: const EdgeInsets.all(20),
        decoration: BoxDecoration(
          color: const Color(0xFF1E293B),
          borderRadius: BorderRadius.circular(16),
          border: Border.all(color: color.withOpacity(0.5), width: 1),
        ),
        child: Row(
          children: [
            Container(
              width: 50, height: 50,
              decoration: BoxDecoration(color: color.withOpacity(0.1), shape: BoxShape.circle),
              child: Center(child: Text(emoji, style: const TextStyle(fontSize: 24))),
            ),
            const SizedBox(width: 15),
            Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text(title, style: const TextStyle(color: Colors.grey, fontSize: 12)),
                const SizedBox(height: 5),
                Text(value, style: const TextStyle(color: Colors.white, fontSize: 16, fontWeight: FontWeight.bold)),
              ],
            )
          ],
        ),
      ),
    );
  }
}
