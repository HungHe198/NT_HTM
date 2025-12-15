# 🛒 Hướng Dẫn Sử Dụng Hệ Thống Bán Hàng Tại Quầy (POS)

## 📋 Giới thiệu
Hệ thống **Bán Hàng Tại Quầy (POS - Point of Sale)** cho phép nhân viên bán hàng tạo đơn bán trực tiếp trên website mà không cần qua quy trình đặt hàng online thông thường. Đây là giải pháp hoàn hảo cho cửa hàng bán lẻ.

---

## 🚀 Cách Truy Cập
1. Đăng nhập với tài khoản **Admin** hoặc **Employee**
2. Vào **Admin Dashboard** → Tìm card **"Bán Tại Quầy"** (icon <i class="bi bi-shop"></i>)
3. Hoặc truy cập trực tiếp: `/Sales`

---

## 📱 Hướng Dẫn Bán Hàng

### 1️⃣ **Tìm Kiếm Sản Phẩm**
```
- Nhập tên sản phẩm hoặc mã SP vào ô tìm kiếm
- Nhấn "Tìm" hoặc Enter
- Chọn sản phẩm từ danh sách gợi ý
```

### 2️⃣ **Chọn Loại Sản Phẩm (Biến Thể)**
```
- Sau khi chọn sản phẩm, danh sách biến thể sẽ hiển thị
- Chọn kích thước, độ cứng, màu sắc... theo nhu cầu
- Hệ thống tự động hiển thị giá và tồn kho
```

### 3️⃣ **Nhập Số Lượng**
```
- Dùng các nút +/- hoặc nhập trực tiếp số lượng
- Tối thiểu 1 cái, tối đa bằng tồn kho
```

### 4️⃣ **Thêm Vào Giỏ Hàng**
```
- Nhấn "Thêm vào giỏ"
- Sản phẩm sẽ hiển thị bên phải trong bảng giỏ hàng
- Bạn có thể thêm nhiều sản phẩm khác nhau
```

### 5️⃣ **Quản Lý Giỏ Hàng**
```
- Xem chi tiết giỏ hàng bên phải
- Thay đổi số lượng trực tiếp trong cột "SL"
- Nhấn <i class="bi bi-trash"></i> để xóa sản phẩm
- Nhấn "Xóa giỏ hàng" để reset hoàn toàn
```

### 6️⃣ **Chọn Khách Hàng**
```
- Nhập số điện thoại khách vào ô "Số điện thoại"
- Nhấn <i class="bi bi-search"></i> để tìm
- Hệ thống sẽ hiển thị: tên, địa chỉ của khách
- Nếu khách không tồn tại, hãy tạo khách hàng mới trước
```

### 7️⃣ **Áp Dụng Mã Giảm Giá (Tùy Chọn)**
```
- Nhập mã voucher vào ô "Mã giảm giá"
- Nhấn <i class="bi bi-check"></i> để kiểm tra
- Nếu hợp lệ: số tiền giảm sẽ hiển thị
- Nếu không hợp lệ: thông báo lỗi sẽ hiển thị
```

### 8️⃣ **Chọn Phương Thức Thanh Toán**
```
- Chọn từ dropdown:
  • Tiền mặt
  • Chuyển khoản
  • Thẻ tín dụng
  • Ví điện tử
  • ...
```

### 9️⃣ **Ghi Chú Đơn (Tùy Chọn)**
```
- Ghi chú thêm về đơn hàng (nếu cần)
- Ví dụ: "Khách yêu cầu cắt nhỏ", "Gói quà tặng"
```

### 🔟 **Tạo Đơn Hàng**
```
- Kiểm tra tóm tắt:
  • Tổng tiền
  • Giảm giá
  • Thành tiền
- Nhấn "Tạo đơn hàng" để hoàn tất
```

---

## 🧾 Sau Khi Tạo Đơn Hàng

### ✅ Thành Công
```
- Hệ thống tự động chuyển sang trang in hóa đơn
- Hóa đơn hiển thị đầy đủ thông tin
- Nhấn "In hóa đơn" để in trên máy in
- Nhấn "Bán hàng tiếp" để quay lại bán sản phẩm khác
```

### 📊 Lịch Sử Bán Hàng
```
- Truy cập "Lịch sử bán hàng" để xem:
  • Tất cả đơn bạn đã tạo
  • Thống kê doanh thu
  • Chi tiết từng đơn hàng
  • Có thể in lại hóa đơn
```

---

## ⚠️ Những Lưu Ý Quan Trọng

| Vấn Đề | Giải Pháp |
|--------|---------|
| Khách không tìm thấy | Kiểm tra số điện thoại chính xác, nếu chưa có thì tạo khách mới |
| Sản phẩm hết hàng | Sản phẩm không hiển thị trong danh sách biến thể nếu hết kho |
| Mã voucher không hợp lệ | Kiểm tra mã đúng, không hết hạn, đủ đơn hàng tối thiểu |
| Không thể tạo đơn | Kiểm tra: chọn khách, chọn PP thanh toán, có sản phẩm trong giỏ |
| Sai số lượng | Sửa trực tiếp trong bảng giỏ hàng hoặc xóa và thêm lại |

---

## 💡 Thực Hành Tốt Nhất

### ✅ Nên Làm
- Kiểm tra tồn kho trước khi bán
- Ghi chú đầy đủ thông tin khách hàng
- In hóa đơn trước khi giao hàng
- Cập nhật lịch sử bán hàng thường xuyên
- Sử dụng mã voucher chính xác

### ❌ Không Nên Làm
- Bán sản phẩm không có trong hệ thống
- Quên ghi khách hàng
- Bán nhiều hơn tồn kho
- Tạo đơn rồi hủy liên tục (gây nhầm lẫn)
- Sử dụng voucher của khách khác

---

## 📞 Hỗ Trợ Kỹ Thuật

Nếu gặp vấn đề:
1. Kiểm tra kết nối internet
2. Làm mới trang (F5)
3. Xóa cache trình duyệt
4. Liên hệ với quản lý hệ thống

---

## 📈 Thống Kê Doanh Thu

Hệ thống tự động ghi nhận:
- ✅ Tổng số đơn bán
- ✅ Tổng doanh thu
- ✅ Tổng giảm giá đã dùng
- ✅ Đơn bán trung bình
- ✅ Chi tiết từng đơn hàng

Xem trong: **Lịch sử bán hàng** → Thống kê

---

**📝 Phiên bản:** 1.0.0  
**⏰ Cập nhật cuối:** December 2024  
**👤 Phát triển bởi:** Team Development
