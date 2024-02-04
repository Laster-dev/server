#include <iostream>
#include <ctime>
#include <sstream>
#include <cstring> // 用于memcpy

std::string generateKeyString() {
    std::time_t now = std::time(nullptr);
    std::tm tmResult;
    localtime_s(&tmResult, &now);

    std::stringstream ss;
    ss << tmResult.tm_year + 1900
        << tmResult.tm_mon + 1
        << tmResult.tm_mday
        << tmResult.tm_hour
        << tmResult.tm_min;

    std::string keyStr = ss.str();

    // 确保密钥字符串长度为16
    while (keyStr.length() < 16) {
        keyStr += keyStr;
    }
    keyStr = keyStr.substr(0, 16);

    return keyStr;
}

void generateAESKey(uint8_t aesKey[16]) {
    std::string keyStr = generateKeyString();

    // 将字符串转换为uint8_t数组
    std::memcpy(aesKey, keyStr.c_str(), 16);
}

int main() {
    uint8_t aesKey[16];
    generateAESKey(aesKey);

    std::cout << "Generated AES Key: ";
    for (int i = 0; i < 16; i++) {
        std::cout << std::hex << +aesKey[i]; // 使用十六进制格式打印
    }
    std::cout << std::endl;

    return 0;
}
