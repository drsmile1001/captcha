# A simple captcha service
簡單的captcha服務

## use
使用方法

Client:
客戶端:
GET /api/Captcha to get captcha id and image in base64.
GET /api/Captcha 以獲取ID和base64圖片。

Server:
伺服器端:
POST /api/Captcha/{id}/Match/{userKeyIn} to verify user key in.
POST /api/Captcha/{id}/Match/{userKeyIn} 以驗證使用者輸入。
