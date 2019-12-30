import os
import base64

def generate_aes_keys() -> (str, str):
    key = os.urandom(32) # 256 bit AES key
    iv = os.urandom(16)
    encoded_key = base64.b64encode(key)
    encoded_iv = base64.b64encode(iv)
    base64_key = str(encoded_key, 'utf-8')
    base64_iv = str(encoded_iv, 'utf-8')
    return (base64_key, base64_iv)

if __name__ == "__main__":
    keys = generate_aes_keys()
    print(f'AES 256 bit key: {keys[0]}\nAES IV: {keys[1]}')
