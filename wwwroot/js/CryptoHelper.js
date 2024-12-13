//window.CryptoHelper = {
//    GenerateRSAKeys: async function () {
//        console.log("Generating RSA keys...");
//        try {
//            const keyPair = await window.crypto.subtle.generateKey(
//                {
//                    name: "RSA-OAEP",
//                    modulusLength: 2048,
//                    publicExponent: new Uint8Array([1, 0, 1]), // 65537
//                    hash: "SHA-512",
//                },
//                true,  // extractable
//                ["encrypt", "decrypt"]
//            );

//            const publicKey = await window.crypto.subtle.exportKey("spki", keyPair.publicKey);
//            const privateKey = await window.crypto.subtle.exportKey("pkcs8", keyPair.privateKey);

//            return {
//                publicKey: ArrayBufferToBase64(publicKey),
//                privateKey: ArrayBufferToBase64(privateKey),
//            };
//        } catch (error) {
//            console.error("Error generating RSA keys:", error);
//            throw error;
//        }
//    },

//    EncryptJWE: async function (data, publicKeyBase64) {
//        console.log("Encrypting data to JWE format...");
//        try {
//            const header = {
//                alg: "RSA-OAEP",
//                enc: "A256GCM",
//            };

//            const encodedHeader = Base64UrlEncode(JSON.stringify(header));

//            // Import public key
//            const publicKey = await window.crypto.subtle.importKey(
//                "spki",
//                Base64ToArrayBuffer(publicKeyBase64),
//                {
//                    name: "RSA-OAEP",
//                    hash: "SHA-512",
//                },
//                false,
//                ["encrypt"]
//            );

//            // Generate AES key for content encryption
//            const aesKey = await window.crypto.subtle.generateKey(
//                { name: "AES-GCM", length: 256 },
//                true,
//                ["encrypt", "decrypt"]
//            );

//            // Encrypt AES key using RSA public key
//            const rawAesKey = await window.crypto.subtle.exportKey("raw", aesKey);
//            const encryptedKey = await window.crypto.subtle.encrypt(
//                { name: "RSA-OAEP" },
//                publicKey,
//                rawAesKey
//            );

//            // Encrypt data using AES-GCM
//            const iv = window.crypto.getRandomValues(new Uint8Array(12)); // 12 bytes IV
//            const encryptedData = await window.crypto.subtle.encrypt(
//                {
//                    name: "AES-GCM",
//                    iv: iv,
//                },
//                aesKey,
//                new TextEncoder().encode(data)
//            );

//            return [
//                encodedHeader,
//                Base64UrlEncode(ArrayBufferToBase64(encryptedKey)),
//                Base64UrlEncode(ArrayBufferToBase64(iv)),
//                Base64UrlEncode(ArrayBufferToBase64(encryptedData))
//            ].join('.');
//        } catch (error) {
//            console.error("Error encrypting JWE:", error);
//            throw error;
//        }
//    },

//    DecryptJWE: async function (jwe, privateKeyBase64) {
//        console.log("Decrypting JWE format...");
//        try {
//            const parts = jwe.split('.');
//            if (parts.length !== 4) {
//                throw new Error("Invalid JWE format");
//            }

//            const [encodedHeader, encryptedKeyBase64Url, ivBase64Url, ciphertextBase64Url] = parts;

//            // Decode header
//            const header = JSON.parse(Base64UrlDecode(encodedHeader));

//            // Import private key
//            const privateKey = await window.crypto.subtle.importKey(
//                "pkcs8",
//                Base64ToArrayBuffer(privateKeyBase64),
//                {
//                    name: "RSA-OAEP",
//                    hash: "SHA-512",
//                },
//                false,
//                ["decrypt"]
//            );

//            // Decrypt AES key
//            const encryptedKey = Base64ToArrayBuffer(Base64UrlDecode(encryptedKeyBase64Url));
//            const rawAesKey = await window.crypto.subtle.decrypt(
//                { name: "RSA-OAEP" },
//                privateKey,
//                encryptedKey
//            );

//            // Import AES key
//            const aesKey = await window.crypto.subtle.importKey(
//                "raw",
//                rawAesKey,
//                { name: "AES-GCM" },
//                false,
//                ["decrypt"]
//            );

//            // Decrypt data
//            const iv = Base64ToArrayBuffer(Base64UrlDecode(ivBase64Url));
//            const ciphertext = Base64ToArrayBuffer(Base64UrlDecode(ciphertextBase64Url));
//            const decryptedData = await window.crypto.subtle.decrypt(
//                {
//                    name: "AES-GCM",
//                    iv: iv,
//                },
//                aesKey,
//                ciphertext
//            );

//            return new TextDecoder().decode(decryptedData);
//        } catch (error) {
//            console.error("Error decrypting JWE:", error);
//            throw error;
//        }
//    },
//};

//// Helper functions
//function ArrayBufferToBase64(buffer) {
//    let binary = '';
//    const bytes = new Uint8Array(buffer);
//    const length = bytes.byteLength;
//    for (let i = 0; i < length; i++) {
//        binary += String.fromCharCode(bytes[i]);
//    }
//    return window.btoa(binary);
//}

//function Base64ToArrayBuffer(base64) {
//    const binary_string = window.atob(base64);
//    const len = binary_string.length;
//    const bytes = new Uint8Array(len);
//    for (let i = 0; i < len; i++) {
//        bytes[i] = binary_string.charCodeAt(i);
//    }
//    return bytes.buffer;
//}

//function Base64UrlEncode(str) {
//    return window.btoa(str).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
//}

//function Base64UrlDecode(str) {
//    return window.atob(str.replace(/-/g, '+').replace(/_/g, '/'));
//}

window.CryptoHelper = {
    generateRSAKeys: async function () {
        try {
            const keyPair = await window.crypto.subtle.generateKey(
                {
                    name: "RSA-OAEP",
                    modulusLength: 2048,
                    publicExponent: new Uint8Array([1, 0, 1]), // 65537
                    hash: "SHA-256",
                },
                true,
                ["encrypt", "decrypt"]
            );

            const publicKey = await window.crypto.subtle.exportKey("spki", keyPair.publicKey);
            const privateKey = await window.crypto.subtle.exportKey("pkcs8", keyPair.privateKey);

            return {
                publicKey: CryptoHelper.arrayBufferToBase64(publicKey),
                privateKey: CryptoHelper.arrayBufferToBase64(privateKey),
            };
        } catch (error) {
            console.error("Error generating RSA keys:", error);
            throw error;
        }
    },
    encryptRSA: async function (data, base64PublicKey) {
        try {
            const publicKey = await CryptoHelper.importPublicKey(base64PublicKey);
            const encodedData = new TextEncoder().encode(data);

            const encryptedData = await window.crypto.subtle.encrypt(
                {
                    name: "RSA-OAEP",
                },
                publicKey,
                encodedData
            );

            return CryptoHelper.arrayBufferToBase64(encryptedData);
        } catch (error) {
            console.error("Error encrypting data:", error);
            throw error;
        }
    },
    decryptRSA: async function (base64Data, base64PrivateKey) {
        try {
            const privateKey = await CryptoHelper.importPrivateKey(base64PrivateKey);
            const data = CryptoHelper.base64ToArrayBuffer(base64Data);

            const decryptedData = await window.crypto.subtle.decrypt(
                {
                    name: "RSA-OAEP",
                },
                privateKey,
                data
            );

            return new TextDecoder().decode(decryptedData);
        } catch (error) {
            console.error("Error decrypting data:", error);
            throw error;
        }
    },
    importPublicKey: async function (base64Key) {
        const binaryKey = CryptoHelper.base64ToArrayBuffer(base64Key);
        return await window.crypto.subtle.importKey(
            "spki",
            binaryKey,
            {
                name: "RSA-OAEP",
                hash: "SHA-256",
            },
            true,
            ["encrypt"]
        );
    },
    importPrivateKey: async function (base64Key) {
        const binaryKey = CryptoHelper.base64ToArrayBuffer(base64Key);
        return await window.crypto.subtle.importKey(
            "pkcs8",
            binaryKey,
            {
                name: "RSA-OAEP",
                hash: "SHA-256",
            },
            true,
            ["decrypt"]
        );
    },
    arrayBufferToBase64: function (buffer) {
        let binary = '';
        const bytes = new Uint8Array(buffer);
        const length = bytes.byteLength;
        for (let i = 0; i < length; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    },
    base64ToArrayBuffer: function (base64) {
        const binaryString = window.atob(base64);
        const len = binaryString.length;
        const bytes = new Uint8Array(len);
        for (let i = 0; i < len; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }
        return bytes.buffer;
    }
};
