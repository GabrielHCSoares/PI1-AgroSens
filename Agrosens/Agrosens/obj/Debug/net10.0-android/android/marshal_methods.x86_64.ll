; ModuleID = 'marshal_methods.x86_64.ll'
source_filename = "marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [257 x ptr] zeroinitializer, align 16

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [771 x i64] [
	i64 u0x001e58127c546039, ; 0: lib_System.Globalization.dll.so => 42
	i64 u0x0024d0f62dee05bd, ; 1: Xamarin.KotlinX.Coroutines.Core.dll => 248
	i64 u0x01109b0e4d99e61f, ; 2: System.ComponentModel.Annotations.dll => 13
	i64 u0x01a68c7fd2565067, ; 3: Xamarin.AndroidX.Graphics.Shapes.Android => 206
	i64 u0x0284512fad379f7e, ; 4: System.Runtime.Handles => 107
	i64 u0x02abedc11addc1ed, ; 5: lib_Mono.Android.Runtime.dll.so => 174
	i64 u0x02f55bf70672f5c8, ; 6: lib_System.IO.FileSystem.DriveInfo.dll.so => 48
	i64 u0x032267b2a94db371, ; 7: lib_Xamarin.AndroidX.AppCompat.dll.so => 183
	i64 u0x03621c804933a890, ; 8: System.Buffers => 7
	i64 u0x0399610510a38a38, ; 9: lib_System.Private.DataContractSerialization.dll.so => 88
	i64 u0x047408741db2431a, ; 10: Xamarin.AndroidX.DynamicAnimation => 201
	i64 u0x0517ef04e06e9f76, ; 11: System.Net.Primitives => 72
	i64 u0x0565d18c6da3de38, ; 12: Xamarin.AndroidX.RecyclerView => 225
	i64 u0x0581db89237110e9, ; 13: lib_System.Collections.dll.so => 12
	i64 u0x05a1c25e78e22d87, ; 14: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 104
	i64 u0x06388ffe9f6c161a, ; 15: System.Xml.Linq.dll => 159
	i64 u0x06600c4c124cb358, ; 16: System.Configuration.dll => 19
	i64 u0x0680a433c781bb3d, ; 17: Xamarin.AndroidX.Collection.Jvm => 189
	i64 u0x069fff96ec92a91d, ; 18: System.Xml.XPath.dll => 164
	i64 u0x070b0847e18dab68, ; 19: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 203
	i64 u0x0739448d84d3b016, ; 20: lib_Xamarin.AndroidX.VectorDrawable.dll.so => 234
	i64 u0x07469f2eecce9e85, ; 21: mscorlib.dll => 170
	i64 u0x07dcdc7460a0c5e4, ; 22: System.Collections.NonGeneric => 10
	i64 u0x088610fc2509f69e, ; 23: lib_Xamarin.AndroidX.VectorDrawable.Animated.dll.so => 235
	i64 u0x08a7c865576bbde7, ; 24: System.Reflection.Primitives => 98
	i64 u0x08c9d051a4a817e5, ; 25: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 199
	i64 u0x08f3c9788ee2153c, ; 26: Xamarin.AndroidX.DrawerLayout => 200
	i64 u0x09138715c92dba90, ; 27: lib_System.ComponentModel.Annotations.dll.so => 13
	i64 u0x092266563089ae3e, ; 28: lib_System.Collections.NonGeneric.dll.so => 10
	i64 u0x09d144a7e214d457, ; 29: System.Security.Cryptography => 129
	i64 u0x09e2b9f743db21a8, ; 30: lib_System.Reflection.Metadata.dll.so => 97
	i64 u0x0abb3e2b271edc45, ; 31: System.Threading.Channels.dll => 143
	i64 u0x0b06b1feab070143, ; 32: System.Formats.Tar => 39
	i64 u0x0b6aff547b84fbe9, ; 33: Xamarin.KotlinX.Serialization.Core.Jvm => 251
	i64 u0x0be2e1f8ce4064ed, ; 34: Xamarin.AndroidX.ViewPager => 237
	i64 u0x0c59ad9fbbd43abe, ; 35: Mono.Android => 175
	i64 u0x0c74af560004e816, ; 36: Microsoft.Win32.Registry.dll => 5
	i64 u0x0c83c82812e96127, ; 37: lib_System.Net.Mail.dll.so => 68
	i64 u0x0d13cd7cce4284e4, ; 38: System.Security.SecureString => 132
	i64 u0x0d63f4f73521c24f, ; 39: lib_Xamarin.AndroidX.SavedState.SavedState.Ktx.dll.so => 229
	i64 u0x0e04e702012f8463, ; 40: Xamarin.AndroidX.Emoji2 => 202
	i64 u0x0e14e73a54dda68e, ; 41: lib_System.Net.NameResolution.dll.so => 69
	i64 u0x0f5e7abaa7cf470a, ; 42: System.Net.HttpListener => 67
	i64 u0x0fb42772d86bc9a0, ; 43: Agrosens => 0
	i64 u0x0ff2f230edd25a94, ; 44: lib_Xamarin.AndroidX.Graphics.Shapes.Android.dll.so => 206
	i64 u0x1001f97bbe242e64, ; 45: System.IO.UnmanagedMemoryStream => 57
	i64 u0x102a31b45304b1da, ; 46: Xamarin.AndroidX.CustomView => 198
	i64 u0x1065c4cb554c3d75, ; 47: System.IO.IsolatedStorage.dll => 52
	i64 u0x10f6cfcbcf801616, ; 48: System.IO.Compression.Brotli => 43
	i64 u0x114443cdcf2091f1, ; 49: System.Security.Cryptography.Primitives => 127
	i64 u0x11a603952763e1d4, ; 50: System.Net.Mail => 68
	i64 u0x11a70d0e1009fb11, ; 51: System.Net.WebSockets.dll => 83
	i64 u0x11f26371eee0d3c1, ; 52: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll.so => 214
	i64 u0x11fbe62d469cc1c8, ; 53: Microsoft.VisualStudio.DesignTools.TapContract.dll => 254
	i64 u0x12128b3f59302d47, ; 54: lib_System.Xml.Serialization.dll.so => 161
	i64 u0x123639456fb056da, ; 55: System.Reflection.Emit.Lightweight.dll => 94
	i64 u0x12521e9764603eaa, ; 56: lib_System.Resources.Reader.dll.so => 101
	i64 u0x125b7f94acb989db, ; 57: Xamarin.AndroidX.RecyclerView.dll => 225
	i64 u0x12d3b63863d4ab0b, ; 58: lib_System.Threading.Overlapped.dll.so => 144
	i64 u0x134eab1061c395ee, ; 59: System.Transactions => 154
	i64 u0x13beedefb0e28a45, ; 60: lib_System.Xml.XmlDocument.dll.so => 165
	i64 u0x13f1e5e209e91af4, ; 61: lib_Java.Interop.dll.so => 172
	i64 u0x1497051b917530bd, ; 62: lib_System.Net.WebSockets.dll.so => 83
	i64 u0x14b78ce3adce0011, ; 63: Microsoft.VisualStudio.DesignTools.TapContract => 254
	i64 u0x14d612a531c79c05, ; 64: Xamarin.JSpecify.dll => 243
	i64 u0x152a448bd1e745a7, ; 65: Microsoft.Win32.Primitives => 4
	i64 u0x1557de0138c445f4, ; 66: lib_Microsoft.Win32.Registry.dll.so => 5
	i64 u0x15bdc156ed462f2f, ; 67: lib_System.IO.FileSystem.dll.so => 51
	i64 u0x15e300c2c1668655, ; 68: System.Resources.Writer.dll => 103
	i64 u0x16bf2a22df043a09, ; 69: System.IO.Pipes.dll => 56
	i64 u0x16ea2b318ad2d830, ; 70: System.Security.Cryptography.Algorithms => 122
	i64 u0x16eeae54c7ebcc08, ; 71: System.Reflection.dll => 100
	i64 u0x17125c9a85b4929f, ; 72: lib_netstandard.dll.so => 171
	i64 u0x1716866f7416792e, ; 73: lib_System.Security.AccessControl.dll.so => 120
	i64 u0x174f71c46216e44a, ; 74: Xamarin.KotlinX.Coroutines.Core => 248
	i64 u0x1752c12f1e1fc00c, ; 75: System.Core => 21
	i64 u0x17f9358913beb16a, ; 76: System.Text.Encodings.Web => 139
	i64 u0x1809fb23f29ba44a, ; 77: lib_System.Reflection.TypeExtensions.dll.so => 99
	i64 u0x18402a709e357f3b, ; 78: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 251
	i64 u0x18a9befae51bb361, ; 79: System.Net.WebClient => 79
	i64 u0x19777fba3c41b398, ; 80: Xamarin.AndroidX.Startup.StartupRuntime.dll => 230
	i64 u0x19a4c090f14ebb66, ; 81: System.Security.Claims => 121
	i64 u0x1a91866a319e9259, ; 82: lib_System.Collections.Concurrent.dll.so => 8
	i64 u0x1aac34d1917ba5d3, ; 83: lib_System.dll.so => 168
	i64 u0x1aea8f1c3b282172, ; 84: lib_System.Net.Ping.dll.so => 71
	i64 u0x1bbdb16cfa73e785, ; 85: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android => 215
	i64 u0x1bc766e07b2b4241, ; 86: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 226
	i64 u0x1c753b5ff15bce1b, ; 87: Mono.Android.Runtime.dll => 174
	i64 u0x1cd47467799d8250, ; 88: System.Threading.Tasks.dll => 148
	i64 u0x1d23eafdc6dc346c, ; 89: System.Globalization.Calendars.dll => 40
	i64 u0x1db6820994506bf5, ; 90: System.IO.FileSystem.AccessControl.dll => 47
	i64 u0x1dbb0c2c6a999acb, ; 91: System.Diagnostics.StackTrace => 30
	i64 u0x1e7c31185e2fb266, ; 92: lib_System.Threading.Tasks.Parallel.dll.so => 147
	i64 u0x1f055d15d807e1b2, ; 93: System.Xml.XmlSerializer => 166
	i64 u0x1f1ed22c1085f044, ; 94: lib_System.Diagnostics.FileVersionInfo.dll.so => 28
	i64 u0x1f61df9c5b94d2c1, ; 95: lib_System.Numerics.dll.so => 86
	i64 u0x1f750bb5421397de, ; 96: lib_Xamarin.AndroidX.Tracing.Tracing.dll.so => 231
	i64 u0x20237ea48006d7a8, ; 97: lib_System.Net.WebClient.dll.so => 79
	i64 u0x209375905fcc1bad, ; 98: lib_System.IO.Compression.Brotli.dll.so => 43
	i64 u0x20fab3cf2dfbc8df, ; 99: lib_System.Diagnostics.Process.dll.so => 29
	i64 u0x2110167c128cba15, ; 100: System.Globalization => 42
	i64 u0x21419508838f7547, ; 101: System.Runtime.CompilerServices.VisualC => 105
	i64 u0x2174319c0d835bc9, ; 102: System.Runtime => 119
	i64 u0x2198e5bc8b7153fa, ; 103: Xamarin.AndroidX.Annotation.Experimental.dll => 181
	i64 u0x219ea1b751a4dee4, ; 104: lib_System.IO.Compression.ZipFile.dll.so => 45
	i64 u0x21cc7e445dcd5469, ; 105: System.Reflection.Emit.ILGeneration => 93
	i64 u0x224538d85ed15a82, ; 106: System.IO.Pipes => 56
	i64 u0x22908438c6bed1af, ; 107: lib_System.Threading.Timer.dll.so => 151
	i64 u0x22fbc14e981e3b45, ; 108: lib_Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll.so => 253
	i64 u0x237be844f1f812c7, ; 109: System.Threading.Thread.dll => 149
	i64 u0x23852b3bdc9f7096, ; 110: System.Resources.ResourceManager => 102
	i64 u0x23986dd7e5d4fc01, ; 111: System.IO.FileSystem.Primitives.dll => 49
	i64 u0x2407aef2bbe8fadf, ; 112: System.Console => 20
	i64 u0x240abe014b27e7d3, ; 113: Xamarin.AndroidX.Core.dll => 194
	i64 u0x247619fe4413f8bf, ; 114: System.Runtime.Serialization.Primitives.dll => 116
	i64 u0x2662c629b96b0b30, ; 115: lib_Xamarin.Kotlin.StdLib.dll.so => 244
	i64 u0x26a670e154a9c54b, ; 116: System.Reflection.Extensions.dll => 96
	i64 u0x26d077d9678fe34f, ; 117: System.IO.dll => 58
	i64 u0x2759af78ab94d39b, ; 118: System.Net.WebSockets => 83
	i64 u0x27b410442fad6cf1, ; 119: Java.Interop.dll => 172
	i64 u0x27b97e0d52c3034a, ; 120: System.Diagnostics.Debug => 26
	i64 u0x2801845a2c71fbfb, ; 121: System.Net.Primitives.dll => 72
	i64 u0x286835e259162700, ; 122: lib_Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll.so => 224
	i64 u0x29f947844fb7fc11, ; 123: Microsoft.Maui.Controls.HotReload.Forms => 252
	i64 u0x2a3b095612184159, ; 124: lib_System.Net.NetworkInformation.dll.so => 70
	i64 u0x2a6507a5ffabdf28, ; 125: System.Diagnostics.TraceSource.dll => 33
	i64 u0x2ad5d6b13b7a3e04, ; 126: System.ComponentModel.DataAnnotations.dll => 14
	i64 u0x2af298f63581d886, ; 127: System.Text.RegularExpressions.dll => 141
	i64 u0x2afc1c4f898552ee, ; 128: lib_System.Formats.Asn1.dll.so => 38
	i64 u0x2b6989d78cba9a15, ; 129: Xamarin.AndroidX.Concurrent.Futures.dll => 190
	i64 u0x2cbd9262ca785540, ; 130: lib_System.Text.Encoding.CodePages.dll.so => 136
	i64 u0x2cc9e1fed6257257, ; 131: lib_System.Reflection.Emit.Lightweight.dll.so => 94
	i64 u0x2cd723e9fe623c7c, ; 132: lib_System.Private.Xml.Linq.dll.so => 90
	i64 u0x2d169d318a968379, ; 133: System.Threading.dll => 152
	i64 u0x2d5ffcae1ad0aaca, ; 134: System.Data.dll => 24
	i64 u0x2db915caf23548d2, ; 135: System.Text.Json.dll => 140
	i64 u0x2dcaa0bb15a4117a, ; 136: System.IO.UnmanagedMemoryStream.dll => 57
	i64 u0x2e2ced2c3c6a1edc, ; 137: lib_System.Threading.AccessControl.dll.so => 142
	i64 u0x2e5a40c319acb800, ; 138: System.IO.FileSystem => 51
	i64 u0x2f02f94df3200fe5, ; 139: System.Diagnostics.Process => 29
	i64 u0x2f2e98e1c89b1aff, ; 140: System.Xml.ReaderWriter => 160
	i64 u0x2f5911d9ba814e4e, ; 141: System.Diagnostics.Tracing => 34
	i64 u0x2f84070a459bc31f, ; 142: lib_System.Xml.dll.so => 167
	i64 u0x309ee9eeec09a71e, ; 143: lib_Xamarin.AndroidX.Fragment.dll.so => 204
	i64 u0x30c6dda129408828, ; 144: System.IO.IsolatedStorage => 52
	i64 u0x31195fef5d8fb552, ; 145: _Microsoft.Android.Resource.Designer.dll => 256
	i64 u0x31496b779ed0663d, ; 146: lib_System.Reflection.DispatchProxy.dll.so => 92
	i64 u0x32243413e774362a, ; 147: Xamarin.AndroidX.CardView.dll => 187
	i64 u0x32319d9df33b1594, ; 148: lib_Xamarin.AndroidX.Graphics.Shapes.dll.so => 205
	i64 u0x3235427f8d12dae1, ; 149: lib_System.Drawing.Primitives.dll.so => 35
	i64 u0x32aa989ff07a84ff, ; 150: lib_System.Xml.ReaderWriter.dll.so => 160
	i64 u0x33829542f112d59b, ; 151: System.Collections.Immutable => 9
	i64 u0x341abc357fbb4ebf, ; 152: lib_System.Net.Sockets.dll.so => 78
	i64 u0x346a212343615ac5, ; 153: lib_System.Linq.AsyncEnumerable.dll.so => 59
	i64 u0x3496c1e2dcaf5ecc, ; 154: lib_System.IO.Pipes.AccessControl.dll.so => 55
	i64 u0x353590da528c9d22, ; 155: System.ComponentModel.Annotations => 13
	i64 u0x3549870798b4cd30, ; 156: lib_Xamarin.AndroidX.ViewPager2.dll.so => 238
	i64 u0x3552fc5d578f0fbf, ; 157: Xamarin.AndroidX.Arch.Core.Common => 185
	i64 u0x355c649948d55d97, ; 158: lib_System.Runtime.Intrinsics.dll.so => 111
	i64 u0x35ea9d1c6834bc8c, ; 159: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 218
	i64 u0x3628ab68db23a01a, ; 160: lib_System.Diagnostics.Tools.dll.so => 32
	i64 u0x3673b042508f5b6b, ; 161: lib_System.Runtime.Extensions.dll.so => 106
	i64 u0x36740f1a8ecdc6c4, ; 162: System.Numerics => 86
	i64 u0x36b2b50fdf589ae2, ; 163: System.Reflection.Emit.Lightweight => 94
	i64 u0x36cada77dc79928b, ; 164: System.IO.MemoryMappedFiles => 53
	i64 u0x374ef46b06791af6, ; 165: System.Reflection.Primitives.dll => 98
	i64 u0x376bf93e521a5417, ; 166: lib_Xamarin.Jetbrains.Annotations.dll.so => 242
	i64 u0x37bc29f3183003b6, ; 167: lib_System.IO.dll.so => 58
	i64 u0x37f55a2ad394d0c1, ; 168: Agrosens.dll => 0
	i64 u0x380134e03b1e160a, ; 169: System.Collections.Immutable.dll => 9
	i64 u0x38049b5c59b39324, ; 170: System.Runtime.CompilerServices.Unsafe => 104
	i64 u0x385c17636bb6fe6e, ; 171: Xamarin.AndroidX.CustomView.dll => 198
	i64 u0x38869c811d74050e, ; 172: System.Net.NameResolution.dll => 69
	i64 u0x393c226616977fdb, ; 173: lib_Xamarin.AndroidX.ViewPager.dll.so => 237
	i64 u0x3ab5859054645f72, ; 174: System.Security.Cryptography.Primitives.dll => 127
	i64 u0x3ad75090c3fac0e9, ; 175: lib_Xamarin.AndroidX.ResourceInspection.Annotation.dll.so => 226
	i64 u0x3ae44ac43a1fbdbb, ; 176: System.Runtime.Serialization => 118
	i64 u0x3b860f9932505633, ; 177: lib_System.Text.Encoding.Extensions.dll.so => 137
	i64 u0x3be99b43dd39dd37, ; 178: Xamarin.AndroidX.SavedState.SavedState.Android => 228
	i64 u0x3c3aafb6b3a00bf6, ; 179: lib_System.Security.Cryptography.X509Certificates.dll.so => 128
	i64 u0x3c4049146b59aa90, ; 180: System.Runtime.InteropServices.JavaScript => 108
	i64 u0x3c7c495f58ac5ee9, ; 181: Xamarin.Kotlin.StdLib => 244
	i64 u0x3c7e5ed3d5db71bb, ; 182: System.Security => 133
	i64 u0x3d1c50cc001a991e, ; 183: Xamarin.Google.Guava.ListenableFuture.dll => 241
	i64 u0x3d2b1913edfc08d7, ; 184: lib_System.Threading.ThreadPool.dll.so => 150
	i64 u0x3d46f0b995082740, ; 185: System.Xml.Linq => 159
	i64 u0x3d9c2a242b040a50, ; 186: lib_Xamarin.AndroidX.Core.dll.so => 194
	i64 u0x3dbb6b9f5ab90fa7, ; 187: lib_Xamarin.AndroidX.DynamicAnimation.dll.so => 201
	i64 u0x3e5441657549b213, ; 188: Xamarin.AndroidX.ResourceInspection.Annotation => 226
	i64 u0x3e57d4d195c53c2e, ; 189: System.Reflection.TypeExtensions => 99
	i64 u0x3e616ab4ed1f3f15, ; 190: lib_System.Data.dll.so => 24
	i64 u0x3f510adf788828dd, ; 191: System.Threading.Tasks.Extensions => 146
	i64 u0x40c98b6bd77346d4, ; 192: Microsoft.VisualBasic.dll => 3
	i64 u0x41833cf766d27d96, ; 193: mscorlib => 170
	i64 u0x41cab042be111c34, ; 194: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 184
	i64 u0x423a9ecc4d905a88, ; 195: lib_System.Resources.ResourceManager.dll.so => 102
	i64 u0x423bf51ae7def810, ; 196: System.Xml.XPath => 164
	i64 u0x42462ff15ddba223, ; 197: System.Resources.Reader.dll => 101
	i64 u0x4291015ff4e5ef71, ; 198: Xamarin.AndroidX.Core.ViewTree.dll => 196
	i64 u0x42a31b86e6ccc3f0, ; 199: System.Diagnostics.Contracts => 25
	i64 u0x42b32a3d24365ec8, ; 200: Xamarin.AndroidX.Graphics.Shapes.Android.dll => 206
	i64 u0x430e95b891249788, ; 201: lib_System.Reflection.Emit.dll.so => 95
	i64 u0x43375950ec7c1b6a, ; 202: netstandard.dll => 171
	i64 u0x434c4e1d9284cdae, ; 203: Mono.Android.dll => 175
	i64 u0x43505013578652a0, ; 204: lib_Xamarin.AndroidX.Activity.Ktx.dll.so => 179
	i64 u0x437d06c381ed575a, ; 205: lib_Microsoft.VisualBasic.dll.so => 3
	i64 u0x43c077442b230f64, ; 206: Xamarin.AndroidX.Tracing.Tracing.Android => 232
	i64 u0x43e8ca5bc927ff37, ; 207: lib_Xamarin.AndroidX.Emoji2.ViewsHelper.dll.so => 203
	i64 u0x448bd33429269b19, ; 208: Microsoft.CSharp => 1
	i64 u0x4499fa3c8e494654, ; 209: lib_System.Runtime.Serialization.Primitives.dll.so => 116
	i64 u0x4515080865a951a5, ; 210: Xamarin.Kotlin.StdLib.dll => 244
	i64 u0x454b4d1e66bb783c, ; 211: Xamarin.AndroidX.Lifecycle.Process => 211
	i64 u0x45c40276a42e283e, ; 212: System.Diagnostics.TraceSource => 33
	i64 u0x45d443f2a29adc37, ; 213: System.AppContext.dll => 6
	i64 u0x46b58f11aea9aa05, ; 214: Xamarin.AndroidX.Graphics.Shapes.dll => 205
	i64 u0x47358bd471172e1d, ; 215: lib_System.Xml.Linq.dll.so => 159
	i64 u0x480c0a47dd42dd81, ; 216: lib_System.IO.MemoryMappedFiles.dll.so => 53
	i64 u0x49e952f19a4e2022, ; 217: System.ObjectModel => 87
	i64 u0x49f9e6948a8131e4, ; 218: lib_Xamarin.AndroidX.VersionedParcelable.dll.so => 236
	i64 u0x4a7a18981dbd56bc, ; 219: System.IO.Compression.FileSystem.dll => 44
	i64 u0x4b07a0ed0ab33ff4, ; 220: System.Runtime.Extensions.dll => 106
	i64 u0x4b576d47ac054f3c, ; 221: System.IO.FileSystem.AccessControl => 47
	i64 u0x4b7b6532ded934b7, ; 222: System.Text.Json => 140
	i64 u0x4c2029a97af23a8d, ; 223: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.Android => 220
	i64 u0x4c7755cf07ad2d5f, ; 224: System.Net.Http.Json.dll => 65
	i64 u0x4cc5f15266470798, ; 225: lib_Xamarin.AndroidX.Loader.dll.so => 221
	i64 u0x4cf6f67dc77aacd2, ; 226: System.Net.NetworkInformation.dll => 70
	i64 u0x4d3183dd245425d4, ; 227: System.Net.WebSockets.Client.dll => 82
	i64 u0x4d479f968a05e504, ; 228: System.Linq.Expressions.dll => 60
	i64 u0x4d55a010ffc4faff, ; 229: System.Private.Xml => 91
	i64 u0x4d5cbe77561c5b2e, ; 230: System.Web.dll => 157
	i64 u0x4d77512dbd86ee4c, ; 231: lib_Xamarin.AndroidX.Arch.Core.Common.dll.so => 185
	i64 u0x4d7793536e79c309, ; 232: System.ServiceProcess => 135
	i64 u0x4d95fccc1f67c7ca, ; 233: System.Runtime.Loader.dll => 112
	i64 u0x4db014bf0ff1c9c1, ; 234: System.Linq.AsyncEnumerable => 59
	i64 u0x4dd9247f1d2c3235, ; 235: Xamarin.AndroidX.Loader.dll => 221
	i64 u0x4e2aeee78e2c4a87, ; 236: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 224
	i64 u0x4e32f00cb0937401, ; 237: Mono.Android.Runtime => 174
	i64 u0x4e5eea4668ac2b18, ; 238: System.Text.Encoding.CodePages => 136
	i64 u0x4ebd0c4b82c5eefc, ; 239: lib_System.Threading.Channels.dll.so => 143
	i64 u0x4ee8eaa9c9c1151a, ; 240: System.Globalization.Calendars => 40
	i64 u0x50c3a29b21050d45, ; 241: System.Linq.Parallel.dll => 61
	i64 u0x5131bbe80989093f, ; 242: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 217
	i64 u0x516324a5050a7e3c, ; 243: System.Net.WebProxy => 81
	i64 u0x516d6f0b21a303de, ; 244: lib_System.Diagnostics.Contracts.dll.so => 25
	i64 u0x51bb8a2afe774e32, ; 245: System.Drawing => 36
	i64 u0x5247c5c32a4140f0, ; 246: System.Resources.Reader => 101
	i64 u0x526bb15e3c386364, ; 247: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 214
	i64 u0x526ce79eb8e90527, ; 248: lib_System.Net.Primitives.dll.so => 72
	i64 u0x52829f00b4467c38, ; 249: lib_System.Data.Common.dll.so => 22
	i64 u0x529ffe06f39ab8db, ; 250: Xamarin.AndroidX.Core => 194
	i64 u0x53978aac584c666e, ; 251: lib_System.Security.Cryptography.Cng.dll.so => 123
	i64 u0x53a96d5c86c9e194, ; 252: System.Net.NetworkInformation => 70
	i64 u0x53be1038a61e8d44, ; 253: System.Runtime.InteropServices.RuntimeInformation.dll => 109
	i64 u0x5435e6f049e9bc37, ; 254: System.Security.Claims.dll => 121
	i64 u0x54795225dd1587af, ; 255: lib_System.Runtime.dll.so => 119
	i64 u0x547a34f14e5f6210, ; 256: Xamarin.AndroidX.Lifecycle.Common.dll => 208
	i64 u0x556e8b63b660ab8b, ; 257: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 209
	i64 u0x5588627c9a108ec9, ; 258: System.Collections.Specialized => 11
	i64 u0x55a898e4f42e3fae, ; 259: Microsoft.VisualBasic.Core.dll => 2
	i64 u0x55fa0c610fe93bb1, ; 260: lib_System.Security.Cryptography.OpenSsl.dll.so => 126
	i64 u0x56442b99bc64bb47, ; 261: System.Runtime.Serialization.Xml.dll => 117
	i64 u0x56a8b26e1aeae27b, ; 262: System.Threading.Tasks.Dataflow => 145
	i64 u0x56f932d61e93c07f, ; 263: System.Globalization.Extensions => 41
	i64 u0x571c5cfbec5ae8e2, ; 264: System.Private.Uri => 89
	i64 u0x576499c9f52fea31, ; 265: Xamarin.AndroidX.Annotation => 180
	i64 u0x579a06fed6eec900, ; 266: System.Private.CoreLib.dll => 177
	i64 u0x57c542c14049b66d, ; 267: System.Diagnostics.DiagnosticSource => 27
	i64 u0x581a8bd5cfda563e, ; 268: System.Threading.Timer => 151
	i64 u0x588c167a79db6bfb, ; 269: lib_Xamarin.Google.ErrorProne.Annotations.dll.so => 240
	i64 u0x5906028ae5151104, ; 270: Xamarin.AndroidX.Activity.Ktx => 179
	i64 u0x595a356d23e8da9a, ; 271: lib_Microsoft.CSharp.dll.so => 1
	i64 u0x59f9e60b9475085f, ; 272: lib_Xamarin.AndroidX.Annotation.Experimental.dll.so => 181
	i64 u0x5a745f5101a75527, ; 273: lib_System.IO.Compression.FileSystem.dll.so => 44
	i64 u0x5a89a886ae30258d, ; 274: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 193
	i64 u0x5a8f6699f4a1caa9, ; 275: lib_System.Threading.dll.so => 152
	i64 u0x5ae9cd33b15841bf, ; 276: System.ComponentModel => 18
	i64 u0x5b54391bdc6fcfe6, ; 277: System.Private.DataContractSerialization => 88
	i64 u0x5b8109e8e14c5e3e, ; 278: System.Globalization.Extensions.dll => 41
	i64 u0x5bdf16b09da116ab, ; 279: Xamarin.AndroidX.Collection => 188
	i64 u0x5c019d5266093159, ; 280: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll.so => 215
	i64 u0x5c30a4a35f9cc8c4, ; 281: lib_System.Reflection.Extensions.dll.so => 96
	i64 u0x5c53c29f5073b0c9, ; 282: System.Diagnostics.FileVersionInfo => 28
	i64 u0x5c87463c575c7616, ; 283: lib_System.Globalization.Extensions.dll.so => 41
	i64 u0x5d0a4a29b02d9d3c, ; 284: System.Net.WebHeaderCollection.dll => 80
	i64 u0x5d40c9b15181641f, ; 285: lib_Xamarin.AndroidX.Emoji2.dll.so => 202
	i64 u0x5d6ca10d35e9485b, ; 286: lib_Xamarin.AndroidX.Concurrent.Futures.dll.so => 190
	i64 u0x5d7ec76c1c703055, ; 287: System.Threading.Tasks.Parallel => 147
	i64 u0x5db0cbbd1028510e, ; 288: lib_System.Runtime.InteropServices.dll.so => 110
	i64 u0x5db30905d3e5013b, ; 289: Xamarin.AndroidX.Collection.Jvm.dll => 189
	i64 u0x5e467bc8f09ad026, ; 290: System.Collections.Specialized.dll => 11
	i64 u0x5e5173b3208d97e7, ; 291: System.Runtime.Handles.dll => 107
	i64 u0x5ea92fdb19ec8c4c, ; 292: System.Text.Encodings.Web.dll => 139
	i64 u0x5eb8046dd40e9ac3, ; 293: System.ComponentModel.Primitives => 16
	i64 u0x5ec272d219c9aba4, ; 294: System.Security.Cryptography.Csp.dll => 124
	i64 u0x5eee1376d94c7f5e, ; 295: System.Net.HttpListener.dll => 67
	i64 u0x5f36ccf5c6a57e24, ; 296: System.Xml.ReaderWriter.dll => 160
	i64 u0x5f4294b9b63cb842, ; 297: System.Data.Common => 22
	i64 u0x5fa6da9c3cd8142a, ; 298: lib_Xamarin.KotlinX.Serialization.Core.dll.so => 250
	i64 u0x5fac98e0b37a5b9d, ; 299: System.Runtime.CompilerServices.Unsafe.dll => 104
	i64 u0x60cd4e33d7e60134, ; 300: Xamarin.KotlinX.Coroutines.Core.Jvm => 249
	i64 u0x60f62d786afcf130, ; 301: System.Memory => 64
	i64 u0x61bb78c89f867353, ; 302: System.IO => 58
	i64 u0x61d88f399afb2f45, ; 303: lib_System.Runtime.Loader.dll.so => 112
	i64 u0x622eef6f9e59068d, ; 304: System.Private.CoreLib => 177
	i64 u0x63cdbd66ac39bb46, ; 305: lib_Microsoft.VisualStudio.DesignTools.XamlTapContract.dll.so => 255
	i64 u0x63d5e3aa4ef9b931, ; 306: Xamarin.KotlinX.Coroutines.Android.dll => 247
	i64 u0x63f1f6883c1e23c2, ; 307: lib_System.Collections.Immutable.dll.so => 9
	i64 u0x6400f68068c1e9f1, ; 308: Xamarin.Google.Android.Material.dll => 239
	i64 u0x640e3b14dbd325c2, ; 309: System.Security.Cryptography.Algorithms.dll => 122
	i64 u0x64587004560099b9, ; 310: System.Reflection => 100
	i64 u0x64b1529a438a3c45, ; 311: lib_System.Runtime.Handles.dll.so => 107
	i64 u0x64b61dd9da8a4d57, ; 312: System.Net.ServerSentEvents.dll => 76
	i64 u0x6565fba2cd8f235b, ; 313: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 218
	i64 u0x65ece51227bfa724, ; 314: lib_System.Runtime.Numerics.dll.so => 113
	i64 u0x661722438787b57f, ; 315: Xamarin.AndroidX.Annotation.Jvm.dll => 182
	i64 u0x6679b2337ee6b22a, ; 316: lib_System.IO.FileSystem.Primitives.dll.so => 49
	i64 u0x667c66a03dd97d40, ; 317: System.Linq.AsyncEnumerable.dll => 59
	i64 u0x6692e924eade1b29, ; 318: lib_System.Console.dll.so => 20
	i64 u0x66a4e5c6a3fb0bae, ; 319: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 217
	i64 u0x66d13304ce1a3efa, ; 320: Xamarin.AndroidX.CursorAdapter => 197
	i64 u0x674303f65d8fad6f, ; 321: lib_System.Net.Quic.dll.so => 73
	i64 u0x6756ca4cad62e9d6, ; 322: lib_Xamarin.AndroidX.ConstraintLayout.Core.dll.so => 192
	i64 u0x67c0802770244408, ; 323: System.Windows.dll => 158
	i64 u0x68100b69286e27cd, ; 324: lib_System.Formats.Tar.dll.so => 39
	i64 u0x6872ec7a2e36b1ac, ; 325: System.Drawing.Primitives.dll => 35
	i64 u0x68bb2c417aa9b61c, ; 326: Xamarin.KotlinX.AtomicFU.dll => 245
	i64 u0x68fbbbe2eb455198, ; 327: System.Formats.Asn1 => 38
	i64 u0x6a4d7577b2317255, ; 328: System.Runtime.InteropServices.dll => 110
	i64 u0x6afcedb171067e2b, ; 329: System.Core.dll => 21
	i64 u0x6bef98e124147c24, ; 330: Xamarin.Jetbrains.Annotations => 242
	i64 u0x6ce874bff138ce2b, ; 331: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 216
	i64 u0x6d70755158ca866e, ; 332: lib_System.ComponentModel.EventBasedAsync.dll.so => 15
	i64 u0x6d7eeca99577fc8b, ; 333: lib_System.Net.WebProxy.dll.so => 81
	i64 u0x6d8515b19946b6a2, ; 334: System.Net.WebProxy.dll => 81
	i64 u0x6d86d56b84c8eb71, ; 335: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 197
	i64 u0x6e79c6bd8627412a, ; 336: Xamarin.AndroidX.SavedState.SavedState.Ktx => 229
	i64 u0x6e838d9a2a6f6c9e, ; 337: lib_System.ValueTuple.dll.so => 155
	i64 u0x6e9965ce1095e60a, ; 338: lib_System.Core.dll.so => 21
	i64 u0x6ffc4967cc47ba57, ; 339: System.IO.FileSystem.Watcher.dll => 50
	i64 u0x701cd46a1c25a5fe, ; 340: System.IO.FileSystem.dll => 51
	i64 u0x71485e7ffdb4b958, ; 341: System.Reflection.Extensions => 96
	i64 u0x71ad672adbe48f35, ; 342: System.ComponentModel.Primitives.dll => 16
	i64 u0x720f102581a4a5c8, ; 343: Xamarin.AndroidX.Core.ViewTree => 196
	i64 u0x725f5a9e82a45c81, ; 344: System.Security.Cryptography.Encoding => 125
	i64 u0x72e0300099accce1, ; 345: System.Xml.XPath.XDocument => 163
	i64 u0x730bfb248998f67a, ; 346: System.IO.Compression.ZipFile => 45
	i64 u0x732b2d67b9e5c47b, ; 347: Xamarin.Google.ErrorProne.Annotations.dll => 240
	i64 u0x73a6be34e822f9d1, ; 348: lib_System.Runtime.Serialization.dll.so => 118
	i64 u0x73e4ce94e2eb6ffc, ; 349: lib_System.Memory.dll.so => 64
	i64 u0x74327ad47b9741a4, ; 350: Xamarin.AndroidX.NavigationEvent.Android.dll => 223
	i64 u0x743a1eccf080489a, ; 351: WindowsBase.dll => 169
	i64 u0x75c326eb821b85c4, ; 352: lib_System.ComponentModel.DataAnnotations.dll.so => 14
	i64 u0x76012e7334db86e5, ; 353: lib_Xamarin.AndroidX.SavedState.dll.so => 227
	i64 u0x76ca07b878f44da0, ; 354: System.Runtime.Numerics.dll => 113
	i64 u0x7736c8a96e51a061, ; 355: lib_Xamarin.AndroidX.Annotation.Jvm.dll.so => 182
	i64 u0x778a805e625329ef, ; 356: System.Linq.Parallel => 61
	i64 u0x779290cc2b801eb7, ; 357: Xamarin.KotlinX.AtomicFU.Jvm => 246
	i64 u0x77d9074d8f33a303, ; 358: lib_System.Net.ServerSentEvents.dll.so => 76
	i64 u0x77f8a4acc2fdc449, ; 359: System.Security.Cryptography.Cng.dll => 123
	i64 u0x782c5d8eb99ff201, ; 360: lib_Microsoft.VisualBasic.Core.dll.so => 2
	i64 u0x78a45e51311409b6, ; 361: Xamarin.AndroidX.Fragment.dll => 204
	i64 u0x78ed4ab8f9d800a1, ; 362: Xamarin.AndroidX.Lifecycle.ViewModel => 216
	i64 u0x7a39601d6f0bb831, ; 363: lib_Xamarin.KotlinX.AtomicFU.dll.so => 245
	i64 u0x7a5207a7c82d30b4, ; 364: lib_Xamarin.JSpecify.dll.so => 243
	i64 u0x7a7e7eddf79c5d26, ; 365: lib_Xamarin.AndroidX.Lifecycle.ViewModel.dll.so => 216
	i64 u0x7a9a57d43b0845fa, ; 366: System.AppContext => 6
	i64 u0x7ad0f4f1e5d08183, ; 367: Xamarin.AndroidX.Collection.dll => 188
	i64 u0x7b13d9eaa944ade8, ; 368: Xamarin.AndroidX.DynamicAnimation.dll => 201
	i64 u0x7bef86a4335c4870, ; 369: System.ComponentModel.TypeConverter => 17
	i64 u0x7c41d387501568ba, ; 370: System.Net.WebClient.dll => 79
	i64 u0x7c482cd79bd24b13, ; 371: lib_Xamarin.AndroidX.ConstraintLayout.dll.so => 191
	i64 u0x7cd2ec8eaf5241cd, ; 372: System.Security.dll => 133
	i64 u0x7cf9ae50dd350622, ; 373: Xamarin.Jetbrains.Annotations.dll => 242
	i64 u0x7d8ee2bdc8e3aad1, ; 374: System.Numerics.Vectors => 85
	i64 u0x7dfc3d6d9d8d7b70, ; 375: System.Collections => 12
	i64 u0x7e2e564fa2f76c65, ; 376: lib_System.Diagnostics.Tracing.dll.so => 34
	i64 u0x7e302e110e1e1346, ; 377: lib_System.Security.Claims.dll.so => 121
	i64 u0x7e4465b3f78ad8d0, ; 378: Xamarin.KotlinX.Serialization.Core.dll => 250
	i64 u0x7e571cad5915e6c3, ; 379: lib_Xamarin.AndroidX.Lifecycle.Process.dll.so => 211
	i64 u0x7e6ac99e4e8df72f, ; 380: System.IO.Hashing => 176
	i64 u0x7e6b1ca712437d7d, ; 381: Xamarin.AndroidX.Emoji2.ViewsHelper => 203
	i64 u0x7e946809d6008ef2, ; 382: lib_System.ObjectModel.dll.so => 87
	i64 u0x7ecc13347c8fd849, ; 383: lib_System.ComponentModel.dll.so => 18
	i64 u0x7f00ddd9b9ca5a13, ; 384: Xamarin.AndroidX.ViewPager.dll => 237
	i64 u0x7fbd557c99b3ce6f, ; 385: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 210
	i64 u0x8076a9a44a2ca331, ; 386: System.Net.Quic => 73
	i64 u0x80b7e726b0280681, ; 387: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 253
	i64 u0x80da183a87731838, ; 388: System.Reflection.Metadata => 97
	i64 u0x812c069d5cdecc17, ; 389: System.dll => 168
	i64 u0x81381be520a60adb, ; 390: Xamarin.AndroidX.Interpolator.dll => 207
	i64 u0x81657cec2b31e8aa, ; 391: System.Net => 84
	i64 u0x8277f2be6b5ce05f, ; 392: Xamarin.AndroidX.AppCompat => 183
	i64 u0x828f06563b30bc50, ; 393: lib_Xamarin.AndroidX.CardView.dll.so => 187
	i64 u0x82920a8d9194a019, ; 394: Xamarin.KotlinX.AtomicFU.Jvm.dll => 246
	i64 u0x82b399cb01b531c4, ; 395: lib_System.Web.dll.so => 157
	i64 u0x82df8f5532a10c59, ; 396: lib_System.Drawing.dll.so => 36
	i64 u0x82f0b6e911d13535, ; 397: lib_System.Transactions.dll.so => 154
	i64 u0x846ce984efea52c7, ; 398: System.Threading.Tasks.Parallel.dll => 147
	i64 u0x84ae73148a4557d2, ; 399: lib_System.IO.Pipes.dll.so => 56
	i64 u0x84b01102c12a9232, ; 400: System.Runtime.Serialization.Json.dll => 115
	i64 u0x850c5ba0b57ce8e7, ; 401: lib_Xamarin.AndroidX.Collection.dll.so => 188
	i64 u0x851d02edd334b044, ; 402: Xamarin.AndroidX.VectorDrawable => 234
	i64 u0x85c919db62150978, ; 403: Xamarin.AndroidX.Transition.dll => 233
	i64 u0x8662aaeb94fef37f, ; 404: lib_System.Dynamic.Runtime.dll.so => 37
	i64 u0x86b62cb077ec4fd7, ; 405: System.Runtime.Serialization.Xml => 117
	i64 u0x86f1940283a92e3d, ; 406: lib_Agrosens.dll.so => 0
	i64 u0x8706ffb12bf3f53d, ; 407: Xamarin.AndroidX.Annotation.Experimental => 181
	i64 u0x872a5b14c18d328c, ; 408: System.ComponentModel.DataAnnotations => 14
	i64 u0x87c69b87d9283884, ; 409: lib_System.Threading.Thread.dll.so => 149
	i64 u0x87f6569b25707834, ; 410: System.IO.Compression.Brotli.dll => 43
	i64 u0x88926583efe7ee86, ; 411: Xamarin.AndroidX.Activity.Ktx.dll => 179
	i64 u0x88ba6bc4f7762b03, ; 412: lib_System.Reflection.dll.so => 100
	i64 u0x88bda98e0cffb7a9, ; 413: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 249
	i64 u0x8930322c7bd8f768, ; 414: netstandard => 171
	i64 u0x897a606c9e39c75f, ; 415: lib_System.ComponentModel.Primitives.dll.so => 16
	i64 u0x898a5c6bc9e47ec1, ; 416: lib_Xamarin.AndroidX.SavedState.SavedState.Android.dll.so => 228
	i64 u0x89911a22005b92b7, ; 417: System.IO.FileSystem.DriveInfo.dll => 48
	i64 u0x89c5188089ec2cd5, ; 418: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 109
	i64 u0x8a19e3dc71b34b2c, ; 419: System.Reflection.TypeExtensions.dll => 99
	i64 u0x8ad229ea26432ee2, ; 420: Xamarin.AndroidX.Loader => 221
	i64 u0x8b4ff5d0fdd5faa1, ; 421: lib_System.Diagnostics.DiagnosticSource.dll.so => 27
	i64 u0x8b541d476eb3774c, ; 422: System.Security.Principal.Windows => 130
	i64 u0x8b8d01333a96d0b5, ; 423: System.Diagnostics.Process.dll => 29
	i64 u0x8cb8f612b633affb, ; 424: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 229
	i64 u0x8cdfdb4ce85fb925, ; 425: lib_System.Security.Principal.Windows.dll.so => 130
	i64 u0x8cdfe7b8f4caa426, ; 426: System.IO.Compression.FileSystem => 44
	i64 u0x8d0f420977c2c1c7, ; 427: Xamarin.AndroidX.CursorAdapter.dll => 197
	i64 u0x8d52f7ea2796c531, ; 428: Xamarin.AndroidX.Emoji2.dll => 202
	i64 u0x8d7b8ab4b3310ead, ; 429: System.Threading => 152
	i64 u0x8da188285aadfe8e, ; 430: System.Collections.Concurrent => 8
	i64 u0x8e8f269ad1e1ff94, ; 431: lib_Xamarin.AndroidX.Tracing.Tracing.Android.dll.so => 232
	i64 u0x8f44b45eb046bbd1, ; 432: System.ServiceModel.Web.dll => 134
	i64 u0x8fbf5b0114c6dcef, ; 433: System.Globalization.dll => 42
	i64 u0x8fcc8c2a81f3d9e7, ; 434: Xamarin.KotlinX.Serialization.Core => 250
	i64 u0x90263f8448b8f572, ; 435: lib_System.Diagnostics.TraceSource.dll.so => 33
	i64 u0x903101b46fb73a04, ; 436: _Microsoft.Android.Resource.Designer => 256
	i64 u0x90393bd4865292f3, ; 437: lib_System.IO.Compression.dll.so => 46
	i64 u0x905e2b8e7ae91ae6, ; 438: System.Threading.Tasks.Extensions.dll => 146
	i64 u0x90634f86c5ebe2b5, ; 439: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 217
	i64 u0x91418dc638b29e68, ; 440: lib_Xamarin.AndroidX.CustomView.dll.so => 198
	i64 u0x9157bd523cd7ed36, ; 441: lib_System.Text.Json.dll.so => 140
	i64 u0x91a74f07b30d37e2, ; 442: System.Linq.dll => 63
	i64 u0x91cb86ea3b17111d, ; 443: System.ServiceModel.Web => 134
	i64 u0x92054e486c0c7ea7, ; 444: System.IO.FileSystem.DriveInfo => 48
	i64 u0x928614058c40c4cd, ; 445: lib_System.Xml.XPath.XDocument.dll.so => 163
	i64 u0x92b138fffca2b01e, ; 446: lib_Xamarin.AndroidX.Arch.Core.Runtime.dll.so => 186
	i64 u0x9388aad9b7ae40ce, ; 447: lib_Xamarin.AndroidX.Lifecycle.Common.dll.so => 208
	i64 u0x941c00d21e5c0679, ; 448: lib_Xamarin.AndroidX.Transition.dll.so => 233
	i64 u0x944077d8ca3c6580, ; 449: System.IO.Compression.dll => 46
	i64 u0x948cffedc8ed7960, ; 450: System.Xml => 167
	i64 u0x94bbeab0d4764588, ; 451: System.IO.Hashing.dll => 176
	i64 u0x94c8990839c4bdb1, ; 452: lib_Xamarin.AndroidX.Interpolator.dll.so => 207
	i64 u0x97b8c771ea3e4220, ; 453: System.ComponentModel.dll => 18
	i64 u0x97e144c9d3c6976e, ; 454: System.Collections.Concurrent.dll => 8
	i64 u0x9843944103683dd3, ; 455: Xamarin.AndroidX.Core.Core.Ktx => 195
	i64 u0x98b05cc81e6f333c, ; 456: Xamarin.AndroidX.SavedState.SavedState.Android.dll => 228
	i64 u0x98d720cc4597562c, ; 457: System.Security.Cryptography.OpenSsl => 126
	i64 u0x991d510397f92d9d, ; 458: System.Linq.Expressions => 60
	i64 u0x996ceeb8a3da3d67, ; 459: System.Threading.Overlapped.dll => 144
	i64 u0x9a01b1da98b6ee10, ; 460: Xamarin.AndroidX.Lifecycle.Runtime.dll => 212
	i64 u0x9b211a749105beac, ; 461: System.Transactions.Local => 153
	i64 u0x9b8734714671022d, ; 462: System.Threading.Tasks.Dataflow.dll => 145
	i64 u0x9bc6aea27fbf034f, ; 463: lib_Xamarin.KotlinX.Coroutines.Core.dll.so => 248
	i64 u0x9bd8cc74558ad4c7, ; 464: Xamarin.KotlinX.AtomicFU => 245
	i64 u0x9c244ac7cda32d26, ; 465: System.Security.Cryptography.X509Certificates.dll => 128
	i64 u0x9c465f280cf43733, ; 466: lib_Xamarin.KotlinX.Coroutines.Android.dll.so => 247
	i64 u0x9c8f6872beab6408, ; 467: System.Xml.XPath.XDocument.dll => 163
	i64 u0x9ce01cf91101ae23, ; 468: System.Xml.XmlDocument => 165
	i64 u0x9d128180c81d7ce6, ; 469: Xamarin.AndroidX.CustomView.PoolingContainer => 199
	i64 u0x9d5dbcf5a48583fe, ; 470: lib_Xamarin.AndroidX.Activity.dll.so => 178
	i64 u0x9e4b95dec42769f7, ; 471: System.Diagnostics.Debug.dll => 26
	i64 u0x9ef542cf1f78c506, ; 472: Xamarin.AndroidX.Lifecycle.LiveData.Core => 210
	i64 u0xa00832eb975f56a8, ; 473: lib_System.Net.dll.so => 84
	i64 u0xa0d8259f4cc284ec, ; 474: lib_System.Security.Cryptography.dll.so => 129
	i64 u0xa0ff9b3e34d92f11, ; 475: lib_System.Resources.Writer.dll.so => 103
	i64 u0xa12fbfb4da97d9f3, ; 476: System.Threading.Timer.dll => 151
	i64 u0xa1440773ee9d341e, ; 477: Xamarin.Google.Android.Material => 239
	i64 u0xa2572680829d2c7c, ; 478: System.IO.Pipelines.dll => 54
	i64 u0xa26597e57ee9c7f6, ; 479: System.Xml.XmlDocument.dll => 165
	i64 u0xa308401900e5bed3, ; 480: lib_mscorlib.dll.so => 170
	i64 u0xa395572e7da6c99d, ; 481: lib_System.Security.dll.so => 133
	i64 u0xa3e683f24b43af6f, ; 482: System.Dynamic.Runtime.dll => 37
	i64 u0xa4145becdee3dc4f, ; 483: Xamarin.AndroidX.VectorDrawable.Animated => 235
	i64 u0xa4edc8f2ceae241a, ; 484: System.Data.Common.dll => 22
	i64 u0xa5494f40f128ce6a, ; 485: System.Runtime.Serialization.Formatters.dll => 114
	i64 u0xa54b74df83dce92b, ; 486: System.Reflection.DispatchProxy => 92
	i64 u0xa5b7152421ed6d98, ; 487: lib_System.IO.FileSystem.Watcher.dll.so => 50
	i64 u0xa5c3844f17b822db, ; 488: lib_System.Linq.Parallel.dll.so => 61
	i64 u0xa5ce5c755bde8cb8, ; 489: lib_System.Security.Cryptography.Csp.dll.so => 124
	i64 u0xa5e599d1e0524750, ; 490: System.Numerics.Vectors.dll => 85
	i64 u0xa5f1ba49b85dd355, ; 491: System.Security.Cryptography.dll => 129
	i64 u0xa61975a5a37873ea, ; 492: lib_System.Xml.XmlSerializer.dll.so => 166
	i64 u0xa66cbee0130865f7, ; 493: lib_WindowsBase.dll.so => 169
	i64 u0xa67dbee13e1df9ca, ; 494: Xamarin.AndroidX.SavedState.dll => 227
	i64 u0xa68a420042bb9b1f, ; 495: Xamarin.AndroidX.DrawerLayout.dll => 200
	i64 u0xa75386b5cb9595aa, ; 496: Xamarin.AndroidX.Lifecycle.Runtime.Android => 213
	i64 u0xa763fbb98df8d9fb, ; 497: lib_Microsoft.Win32.Primitives.dll.so => 4
	i64 u0xa78ce3745383236a, ; 498: Xamarin.AndroidX.Lifecycle.Common.Jvm => 209
	i64 u0xa7eab29ed44b4e7a, ; 499: Mono.Android.Export => 173
	i64 u0xa8195217cbf017b7, ; 500: Microsoft.VisualBasic.Core => 2
	i64 u0xa859a95830f367ff, ; 501: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll.so => 218
	i64 u0xa8b52f21e0dbe690, ; 502: System.Runtime.Serialization.dll => 118
	i64 u0xa8c84ce526c2b4bd, ; 503: Microsoft.VisualStudio.DesignTools.XamlTapContract.dll => 255
	i64 u0xa8ee4ed7de2efaee, ; 504: Xamarin.AndroidX.Annotation.dll => 180
	i64 u0xa95590e7c57438a4, ; 505: System.Configuration => 19
	i64 u0xaa443ac34067eeef, ; 506: System.Private.Xml.dll => 91
	i64 u0xaa52de307ef5d1dd, ; 507: System.Net.Http => 66
	i64 u0xaa9a7b0214a5cc5c, ; 508: System.Diagnostics.StackTrace.dll => 30
	i64 u0xaaeebf9c8d9c28aa, ; 509: Xamarin.AndroidX.Graphics.Shapes => 205
	i64 u0xab9af77b5b67a0b8, ; 510: Xamarin.AndroidX.ConstraintLayout.Core => 192
	i64 u0xab9c1b2687d86b0b, ; 511: lib_System.Linq.Expressions.dll.so => 60
	i64 u0xac2af3fa195a15ce, ; 512: System.Runtime.Numerics => 113
	i64 u0xac5376a2a538dc10, ; 513: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 210
	i64 u0xac5acae88f60357e, ; 514: System.Diagnostics.Tools.dll => 32
	i64 u0xac79c7e46047ad98, ; 515: System.Security.Principal.Windows.dll => 130
	i64 u0xac98d31068e24591, ; 516: System.Xml.XDocument => 162
	i64 u0xacdd9e4180d56dda, ; 517: Xamarin.AndroidX.Concurrent.Futures => 190
	i64 u0xacf42eea7ef9cd12, ; 518: System.Threading.Channels => 143
	i64 u0xadbb53caf78a79d2, ; 519: System.Web.HttpUtility => 156
	i64 u0xadc90ab061a9e6e4, ; 520: System.ComponentModel.TypeConverter.dll => 17
	i64 u0xadf4cf30debbeb9a, ; 521: System.Net.ServicePoint.dll => 77
	i64 u0xadf511667bef3595, ; 522: System.Net.Security => 75
	i64 u0xae0aaa94fdcfce0f, ; 523: System.ComponentModel.EventBasedAsync.dll => 15
	i64 u0xae282bcd03739de7, ; 524: Java.Interop => 172
	i64 u0xae53579c90db1107, ; 525: System.ObjectModel.dll => 87
	i64 u0xaec7c0c7e2ed4575, ; 526: lib_Xamarin.KotlinX.AtomicFU.Jvm.dll.so => 246
	i64 u0xaf732d0b2193b8f5, ; 527: System.Security.Cryptography.OpenSsl.dll => 126
	i64 u0xafe29f45095518e7, ; 528: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 219
	i64 u0xb03ae931fb25607e, ; 529: Xamarin.AndroidX.ConstraintLayout => 191
	i64 u0xb0ac21bec8f428c5, ; 530: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll => 215
	i64 u0xb0bb43dc52ea59f9, ; 531: System.Diagnostics.Tracing.dll => 34
	i64 u0xb1dd05401aa8ee63, ; 532: System.Security.AccessControl => 120
	i64 u0xb220631954820169, ; 533: System.Text.RegularExpressions => 141
	i64 u0xb2376e1dbf8b4ed7, ; 534: System.Security.Cryptography.Csp => 124
	i64 u0xb2a1959fe95c5402, ; 535: lib_System.Runtime.InteropServices.JavaScript.dll.so => 108
	i64 u0xb3011a0a57f7ffb2, ; 536: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 253
	i64 u0xb34f09b530d826d6, ; 537: Xamarin.AndroidX.NavigationEvent.dll => 222
	i64 u0xb3874072ee0ecf8c, ; 538: Xamarin.AndroidX.VectorDrawable.Animated.dll => 235
	i64 u0xb3f0a0fcda8d3ebc, ; 539: Xamarin.AndroidX.CardView => 187
	i64 u0xb4bd7015ecee9d86, ; 540: System.IO.Pipelines => 54
	i64 u0xb4c53d9749c5f226, ; 541: lib_System.IO.FileSystem.AccessControl.dll.so => 47
	i64 u0xb4ff710863453fda, ; 542: System.Diagnostics.FileVersionInfo.dll => 28
	i64 u0xb54092076b15e062, ; 543: System.Threading.AccessControl => 142
	i64 u0xb5c38bf497a4cfe2, ; 544: lib_System.Threading.Tasks.dll.so => 148
	i64 u0xb5ea31d5244c6626, ; 545: System.Threading.ThreadPool.dll => 150
	i64 u0xb7212c4683a94afe, ; 546: System.Drawing.Primitives => 35
	i64 u0xb81a2c6e0aee50fe, ; 547: lib_System.Private.CoreLib.dll.so => 177
	i64 u0xb8c60af47c08d4da, ; 548: System.Net.ServicePoint => 77
	i64 u0xb8e68d20aad91196, ; 549: lib_System.Xml.XPath.dll.so => 164
	i64 u0xb9185c33a1643eed, ; 550: Microsoft.CSharp.dll => 1
	i64 u0xb960d6b2200ba320, ; 551: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.Android.dll => 220
	i64 u0xba4670aa94a2b3c6, ; 552: lib_System.Xml.XDocument.dll.so => 162
	i64 u0xba48785529705af9, ; 553: System.Collections.dll => 12
	i64 u0xba965b8c86359996, ; 554: lib_System.Windows.dll.so => 158
	i64 u0xbb286883bc35db36, ; 555: System.Transactions.dll => 154
	i64 u0xbb65706fde942ce3, ; 556: System.Net.Sockets => 78
	i64 u0xbba28979413cad9e, ; 557: lib_System.Runtime.CompilerServices.VisualC.dll.so => 105
	i64 u0xbbd180354b67271a, ; 558: System.Runtime.Serialization.Formatters => 114
	i64 u0xbc260cdba33291a3, ; 559: Xamarin.AndroidX.Arch.Core.Common.dll => 185
	i64 u0xbd0e2c0d55246576, ; 560: System.Net.Http.dll => 66
	i64 u0xbd3fbd85b9e1cb29, ; 561: lib_System.Net.HttpListener.dll.so => 67
	i64 u0xbd437a2cdb333d0d, ; 562: Xamarin.AndroidX.ViewPager2 => 238
	i64 u0xbd4f572d2bd0a789, ; 563: System.IO.Compression.ZipFile.dll => 45
	i64 u0xbd877b14d0b56392, ; 564: System.Runtime.Intrinsics.dll => 111
	i64 u0xbe65a49036345cf4, ; 565: lib_System.Buffers.dll.so => 7
	i64 u0xbee38d4a88835966, ; 566: Xamarin.AndroidX.AppCompat.AppCompatResources => 184
	i64 u0xbef9919db45b4ca7, ; 567: System.IO.Pipes.AccessControl => 55
	i64 u0xbf0fa68611139208, ; 568: lib_Xamarin.AndroidX.Annotation.dll.so => 180
	i64 u0xbfc1e1fb3095f2b3, ; 569: lib_System.Net.Http.Json.dll.so => 65
	i64 u0xc07cadab29efeba0, ; 570: Xamarin.AndroidX.Core.Core.Ktx.dll => 195
	i64 u0xc0d928351ab5ca77, ; 571: System.Console.dll => 20
	i64 u0xc0f5a221a9383aea, ; 572: System.Runtime.Intrinsics => 111
	i64 u0xc111030af54d7191, ; 573: System.Resources.Writer => 103
	i64 u0xc12b8b3afa48329c, ; 574: lib_System.Linq.dll.so => 63
	i64 u0xc183ca0b74453aa9, ; 575: lib_System.Threading.Tasks.Dataflow.dll.so => 145
	i64 u0xc1ff9ae3cdb6e1e6, ; 576: Xamarin.AndroidX.Activity.dll => 178
	i64 u0xc26c064effb1dea9, ; 577: System.Buffers.dll => 7
	i64 u0xc2902f6cf5452577, ; 578: lib_Mono.Android.Export.dll.so => 173
	i64 u0xc2a3bca55b573141, ; 579: System.IO.FileSystem.Watcher => 50
	i64 u0xc2bcfec99f69365e, ; 580: Xamarin.AndroidX.ViewPager2.dll => 238
	i64 u0xc30b52815b58ac2c, ; 581: lib_System.Runtime.Serialization.Xml.dll.so => 117
	i64 u0xc36d7d89c652f455, ; 582: System.Threading.Overlapped => 144
	i64 u0xc3c86c1e5e12f03d, ; 583: WindowsBase => 169
	i64 u0xc421b61fd853169d, ; 584: lib_System.Net.WebSockets.Client.dll.so => 82
	i64 u0xc463e077917aa21d, ; 585: System.Runtime.Serialization.Json => 115
	i64 u0xc4d3858ed4d08512, ; 586: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 219
	i64 u0xc50fded0ded1418c, ; 587: lib_System.ComponentModel.TypeConverter.dll.so => 17
	i64 u0xc519125d6bc8fb11, ; 588: lib_System.Net.Requests.dll.so => 74
	i64 u0xc5325b2fcb37446f, ; 589: lib_System.Private.Xml.dll.so => 91
	i64 u0xc5a0f4b95a699af7, ; 590: lib_System.Private.Uri.dll.so => 89
	i64 u0xc5cdcd5b6277579e, ; 591: lib_System.Security.Cryptography.Algorithms.dll.so => 122
	i64 u0xc5ec286825cb0bf4, ; 592: Xamarin.AndroidX.Tracing.Tracing => 231
	i64 u0xc6706bc8aa7fe265, ; 593: Xamarin.AndroidX.Annotation.Jvm => 182
	i64 u0xc7c01e7d7c93a110, ; 594: System.Text.Encoding.Extensions.dll => 137
	i64 u0xc7ce851898a4548e, ; 595: lib_System.Web.HttpUtility.dll.so => 156
	i64 u0xc809d4089d2556b2, ; 596: System.Runtime.InteropServices.JavaScript.dll => 108
	i64 u0xc858a28d9ee5a6c5, ; 597: lib_System.Collections.Specialized.dll.so => 11
	i64 u0xc8ac7c6bf1c2ec51, ; 598: System.Reflection.DispatchProxy.dll => 92
	i64 u0xc9c62c8f354ac568, ; 599: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 31
	i64 u0xca5801070d9fccfb, ; 600: System.Text.Encoding => 138
	i64 u0xcadbc92899a777f0, ; 601: Xamarin.AndroidX.Startup.StartupRuntime => 230
	i64 u0xcbb5f80c7293e696, ; 602: lib_System.Globalization.Calendars.dll.so => 40
	i64 u0xcbd4fdd9cef4a294, ; 603: lib__Microsoft.Android.Resource.Designer.dll.so => 256
	i64 u0xcc2876b32ef2794c, ; 604: lib_System.Text.RegularExpressions.dll.so => 141
	i64 u0xcc5c3bb714c4561e, ; 605: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 249
	i64 u0xcc76886e09b88260, ; 606: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 251
	i64 u0xcc9fa2923aa1c9ef, ; 607: System.Diagnostics.Contracts.dll => 25
	i64 u0xccae9bb73e2326bd, ; 608: lib_System.IO.Hashing.dll.so => 176
	i64 u0xcd10a42808629144, ; 609: System.Net.Requests => 74
	i64 u0xcdca1b920e9f53ba, ; 610: Xamarin.AndroidX.Interpolator => 207
	i64 u0xcde1fa22dc303670, ; 611: Microsoft.VisualStudio.DesignTools.XamlTapContract => 255
	i64 u0xcf23d8093f3ceadf, ; 612: System.Diagnostics.DiagnosticSource.dll => 27
	i64 u0xcf5ff6b6b2c4c382, ; 613: System.Net.Mail.dll => 68
	i64 u0xcf8fc898f98b0d34, ; 614: System.Private.Xml.Linq => 90
	i64 u0xd04b5f59ed596e31, ; 615: System.Reflection.Metadata.dll => 97
	i64 u0xd063299fcfc0c93f, ; 616: lib_System.Runtime.Serialization.Json.dll.so => 115
	i64 u0xd0de8a113e976700, ; 617: System.Diagnostics.TextWriterTraceListener => 31
	i64 u0xd0fc33d5ae5d4cb8, ; 618: System.Runtime.Extensions => 106
	i64 u0xd1194e1d8a8de83c, ; 619: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 209
	i64 u0xd12beacdfc14f696, ; 620: System.Dynamic.Runtime => 37
	i64 u0xd198e7ce1b6a8344, ; 621: System.Net.Quic.dll => 73
	i64 u0xd3144156a3727ebe, ; 622: Xamarin.Google.Guava.ListenableFuture => 241
	i64 u0xd333d0af9e423810, ; 623: System.Runtime.InteropServices => 110
	i64 u0xd33a415cb4278969, ; 624: System.Security.Cryptography.Encoding.dll => 125
	i64 u0xd3426d966bb704f5, ; 625: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 184
	i64 u0xd3651b6fc3125825, ; 626: System.Private.Uri.dll => 89
	i64 u0xd3801faafafb7698, ; 627: System.Private.DataContractSerialization.dll => 88
	i64 u0xd3edcc1f25459a50, ; 628: System.Reflection.Emit => 95
	i64 u0xd4fa0abb79079ea9, ; 629: System.Security.Principal.dll => 131
	i64 u0xd5507e11a2b2839f, ; 630: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 219
	i64 u0xd5d04bef8478ea19, ; 631: Xamarin.AndroidX.Tracing.Tracing.dll => 231
	i64 u0xd65786d27a4ad960, ; 632: lib_Microsoft.Maui.Controls.HotReload.Forms.dll.so => 252
	i64 u0xd6694f8359737e4e, ; 633: Xamarin.AndroidX.SavedState => 227
	i64 u0xd6949e129339eae5, ; 634: lib_Xamarin.AndroidX.Core.Core.Ktx.dll.so => 195
	i64 u0xd6de019f6af72435, ; 635: Xamarin.AndroidX.ConstraintLayout.Core.dll => 192
	i64 u0xd6f6d366d4896aef, ; 636: lib_Xamarin.AndroidX.NavigationEvent.Android.dll.so => 223
	i64 u0xd72c760af136e863, ; 637: System.Xml.XmlSerializer.dll => 166
	i64 u0xd753f071e44c2a03, ; 638: lib_System.Security.SecureString.dll.so => 132
	i64 u0xd7f0088bc5ad71f2, ; 639: Xamarin.AndroidX.VersionedParcelable => 236
	i64 u0xd8fb25e28ae30a12, ; 640: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 224
	i64 u0xdad05a11827959a3, ; 641: System.Collections.NonGeneric.dll => 10
	i64 u0xdaefdfe71aa53cf9, ; 642: System.IO.FileSystem.Primitives => 49
	i64 u0xdb58816721c02a59, ; 643: lib_System.Reflection.Emit.ILGeneration.dll.so => 93
	i64 u0xdbf2a779fbc3ac31, ; 644: System.Transactions.Local.dll => 153
	i64 u0xdbf9607a441b4505, ; 645: System.Linq => 63
	i64 u0xdbfc90157a0de9b0, ; 646: lib_System.Text.Encoding.dll.so => 138
	i64 u0xdc75032002d1a212, ; 647: lib_System.Transactions.Local.dll.so => 153
	i64 u0xdca8be7403f92d4f, ; 648: lib_System.Linq.Queryable.dll.so => 62
	i64 u0xdd2b722d78ef5f43, ; 649: System.Runtime.dll => 119
	i64 u0xdd67031857c72f96, ; 650: lib_System.Text.Encodings.Web.dll.so => 139
	i64 u0xdd70765ad6162057, ; 651: Xamarin.JSpecify => 243
	i64 u0xdd92e229ad292030, ; 652: System.Numerics.dll => 86
	i64 u0xde110ae80fa7c2e2, ; 653: System.Xml.XDocument.dll => 162
	i64 u0xde4726fcdf63a198, ; 654: Xamarin.AndroidX.Transition => 233
	i64 u0xde572c2b2fb32f93, ; 655: lib_System.Threading.Tasks.Extensions.dll.so => 146
	i64 u0xdf4b773de8fb1540, ; 656: System.Net.dll => 84
	i64 u0xdf9c7682560a9629, ; 657: System.Net.ServerSentEvents => 76
	i64 u0xdfa254ebb4346068, ; 658: System.Net.Ping => 71
	i64 u0xe0142572c095a480, ; 659: Xamarin.AndroidX.AppCompat.dll => 183
	i64 u0xe021eaa401792a05, ; 660: System.Text.Encoding.dll => 138
	i64 u0xe02f89350ec78051, ; 661: Xamarin.AndroidX.CoordinatorLayout.dll => 193
	i64 u0xe10b760bb1462e7a, ; 662: lib_System.Security.Cryptography.Primitives.dll.so => 127
	i64 u0xe1566bbdb759c5af, ; 663: Microsoft.Maui.Controls.HotReload.Forms.dll => 252
	i64 u0xe192a588d4410686, ; 664: lib_System.IO.Pipelines.dll.so => 54
	i64 u0xe1a08bd3fa539e0d, ; 665: System.Runtime.Loader => 112
	i64 u0xe1a77eb8831f7741, ; 666: System.Security.SecureString.dll => 132
	i64 u0xe1b52f9f816c70ef, ; 667: System.Private.Xml.Linq.dll => 90
	i64 u0xe1e199c8ab02e356, ; 668: System.Data.DataSetExtensions.dll => 23
	i64 u0xe1ecfdb7fff86067, ; 669: System.Net.Security.dll => 75
	i64 u0xe2252a80fe853de4, ; 670: lib_System.Security.Principal.dll.so => 131
	i64 u0xe22fa4c9c645db62, ; 671: System.Diagnostics.TextWriterTraceListener.dll => 31
	i64 u0xe2420585aeceb728, ; 672: System.Net.Requests.dll => 74
	i64 u0xe26692647e6bcb62, ; 673: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 214
	i64 u0xe2ad448dee50fbdf, ; 674: System.Xml.Serialization => 161
	i64 u0xe2d920f978f5d85c, ; 675: System.Data.DataSetExtensions => 23
	i64 u0xe2e426c7714fa0bc, ; 676: Microsoft.Win32.Primitives.dll => 4
	i64 u0xe332bacb3eb4a806, ; 677: Mono.Android.Export.dll => 173
	i64 u0xe3b7cbae5ad66c75, ; 678: lib_System.Security.Cryptography.Encoding.dll.so => 125
	i64 u0xe4292b48f3224d5b, ; 679: lib_Xamarin.AndroidX.Core.ViewTree.dll.so => 196
	i64 u0xe4dd7925d04f7f6b, ; 680: lib_Xamarin.AndroidX.NavigationEvent.dll.so => 222
	i64 u0xe4f74a0b5bf9703f, ; 681: System.Runtime.Serialization.Primitives => 116
	i64 u0xe5434e8a119ceb69, ; 682: lib_Mono.Android.dll.so => 175
	i64 u0xe55703b9ce5c038a, ; 683: System.Diagnostics.Tools => 32
	i64 u0xe57013c8afc270b5, ; 684: Microsoft.VisualBasic => 3
	i64 u0xe62913cc36bc07ec, ; 685: System.Xml.dll => 167
	i64 u0xe7bea09c4900a191, ; 686: Xamarin.AndroidX.VectorDrawable.dll => 234
	i64 u0xe7e03cc18dcdeb49, ; 687: lib_System.Diagnostics.StackTrace.dll.so => 30
	i64 u0xe7e147ff99a7a380, ; 688: lib_System.Configuration.dll.so => 19
	i64 u0xe7ef84b36819564e, ; 689: Xamarin.AndroidX.NavigationEvent.Android => 223
	i64 u0xe86b0df4ba9e5db8, ; 690: lib_Xamarin.AndroidX.Lifecycle.Runtime.Android.dll.so => 213
	i64 u0xe896622fe0902957, ; 691: System.Reflection.Emit.dll => 95
	i64 u0xe89a2a9ef110899b, ; 692: System.Drawing.dll => 36
	i64 u0xe8c5f8c100b5934b, ; 693: Microsoft.Win32.Registry => 5
	i64 u0xe98163eb702ae5c5, ; 694: Xamarin.AndroidX.Arch.Core.Runtime => 186
	i64 u0xe994f23ba4c143e5, ; 695: Xamarin.KotlinX.Coroutines.Android => 247
	i64 u0xe9b9c8c0458fd92a, ; 696: System.Windows => 158
	i64 u0xe9d166d87a7f2bdb, ; 697: lib_Xamarin.AndroidX.Startup.StartupRuntime.dll.so => 230
	i64 u0xea5a4efc2ad81d1b, ; 698: Xamarin.Google.ErrorProne.Annotations => 240
	i64 u0xeb2313fe9d65b785, ; 699: Xamarin.AndroidX.ConstraintLayout.dll => 191
	i64 u0xed19c616b3fcb7eb, ; 700: Xamarin.AndroidX.VersionedParcelable.dll => 236
	i64 u0xed60c6fa891c051a, ; 701: lib_Microsoft.VisualStudio.DesignTools.TapContract.dll.so => 254
	i64 u0xedc4817167106c23, ; 702: System.Net.Sockets.dll => 78
	i64 u0xedc632067fb20ff3, ; 703: System.Memory.dll => 64
	i64 u0xee81f5b3f1c4f83b, ; 704: System.Threading.ThreadPool => 150
	i64 u0xeeb7ebb80150501b, ; 705: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 189
	i64 u0xeefc635595ef57f0, ; 706: System.Security.Cryptography.Cng => 123
	i64 u0xef03b1b5a04e9709, ; 707: System.Text.Encoding.CodePages.dll => 136
	i64 u0xef5bcbe61622ee5f, ; 708: Xamarin.AndroidX.Tracing.Tracing.Android.dll => 232
	i64 u0xef602c523fe2e87a, ; 709: lib_Xamarin.Google.Guava.ListenableFuture.dll.so => 241
	i64 u0xefd1e0c4e5c9b371, ; 710: System.Resources.ResourceManager.dll => 102
	i64 u0xefe8f8d5ed3c72ea, ; 711: System.Formats.Tar.dll => 39
	i64 u0xefec0b7fdc57ec42, ; 712: Xamarin.AndroidX.Activity => 178
	i64 u0xeff59cbde4363ec3, ; 713: System.Threading.AccessControl.dll => 142
	i64 u0xf09e47b6ae914f6e, ; 714: System.Net.NameResolution => 69
	i64 u0xf0ac2b489fed2e35, ; 715: lib_System.Diagnostics.Debug.dll.so => 26
	i64 u0xf0bb49dadd3a1fe1, ; 716: lib_System.Net.ServicePoint.dll.so => 77
	i64 u0xf0de2537ee19c6ca, ; 717: lib_System.Net.WebHeaderCollection.dll.so => 80
	i64 u0xf1138779fa181c68, ; 718: lib_Xamarin.AndroidX.Lifecycle.Runtime.dll.so => 212
	i64 u0xf161f4f3c3b7e62c, ; 719: System.Data => 24
	i64 u0xf16eb650d5a464bc, ; 720: System.ValueTuple => 155
	i64 u0xf1c4b4005493d871, ; 721: System.Formats.Asn1.dll => 38
	i64 u0xf22514cfad2d598b, ; 722: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.Android.dll.so => 220
	i64 u0xf2feea356ba760af, ; 723: Xamarin.AndroidX.Arch.Core.Runtime.dll => 186
	i64 u0xf300e085f8acd238, ; 724: lib_System.ServiceProcess.dll.so => 135
	i64 u0xf33cd2d62dbe52f9, ; 725: Xamarin.AndroidX.NavigationEvent => 222
	i64 u0xf34e52b26e7e059d, ; 726: System.Runtime.CompilerServices.VisualC.dll => 105
	i64 u0xf37221fda4ef8830, ; 727: lib_Xamarin.Google.Android.Material.dll.so => 239
	i64 u0xf3ad9b8fb3eefd12, ; 728: lib_System.IO.UnmanagedMemoryStream.dll.so => 57
	i64 u0xf3ddfe05336abf29, ; 729: System => 168
	i64 u0xf408654b2a135055, ; 730: System.Reflection.Emit.ILGeneration.dll => 93
	i64 u0xf4103170a1de5bd0, ; 731: System.Linq.Queryable.dll => 62
	i64 u0xf42d20c23173d77c, ; 732: lib_System.ServiceModel.Web.dll.so => 134
	i64 u0xf4c1dd70a5496a17, ; 733: System.IO.Compression => 46
	i64 u0xf4ecf4b9afc64781, ; 734: System.ServiceProcess.dll => 135
	i64 u0xf4eeeaa566e9b970, ; 735: lib_Xamarin.AndroidX.CustomView.PoolingContainer.dll.so => 199
	i64 u0xf518f63ead11fcd1, ; 736: System.Threading.Tasks => 148
	i64 u0xf5fc7602fe27b333, ; 737: System.Net.WebHeaderCollection => 80
	i64 u0xf6077741019d7428, ; 738: Xamarin.AndroidX.CoordinatorLayout => 193
	i64 u0xf6742cbf457c450b, ; 739: Xamarin.AndroidX.Lifecycle.Runtime.Android.dll => 213
	i64 u0xf70c0a7bf8ccf5af, ; 740: System.Web => 157
	i64 u0xf7e2cac4c45067b3, ; 741: lib_System.Numerics.Vectors.dll.so => 85
	i64 u0xf8aac5ea82de1348, ; 742: System.Linq.Queryable => 62
	i64 u0xf8b77539b362d3ba, ; 743: lib_System.Reflection.Primitives.dll.so => 98
	i64 u0xf8e045dc345b2ea3, ; 744: lib_Xamarin.AndroidX.RecyclerView.dll.so => 225
	i64 u0xf915dc29808193a1, ; 745: System.Web.HttpUtility.dll => 156
	i64 u0xf9be54c8bcf8ff3b, ; 746: System.Security.AccessControl.dll => 120
	i64 u0xfa0e82300e67f913, ; 747: lib_System.AppContext.dll.so => 6
	i64 u0xfa2fdb27e8a2c8e8, ; 748: System.ComponentModel.EventBasedAsync => 15
	i64 u0xfa3f278f288b0e84, ; 749: lib_System.Net.Security.dll.so => 75
	i64 u0xfa645d91e9fc4cba, ; 750: System.Threading.Thread => 149
	i64 u0xfad4d2c770e827f9, ; 751: lib_System.IO.IsolatedStorage.dll.so => 52
	i64 u0xfb06dd2338e6f7c4, ; 752: System.Net.Ping.dll => 71
	i64 u0xfb087abe5365e3b7, ; 753: lib_System.Data.DataSetExtensions.dll.so => 23
	i64 u0xfb846e949baff5ea, ; 754: System.Xml.Serialization.dll => 161
	i64 u0xfbad3e4ce4b98145, ; 755: System.Security.Cryptography.X509Certificates => 128
	i64 u0xfbf0a31c9fc34bc4, ; 756: lib_System.Net.Http.dll.so => 66
	i64 u0xfc6b7527cc280b3f, ; 757: lib_System.Runtime.Serialization.Formatters.dll.so => 114
	i64 u0xfc82690c2fe2735c, ; 758: Xamarin.AndroidX.Lifecycle.Process.dll => 211
	i64 u0xfc93fc307d279893, ; 759: System.IO.Pipes.AccessControl.dll => 55
	i64 u0xfcd302092ada6328, ; 760: System.IO.MemoryMappedFiles.dll => 53
	i64 u0xfd22f00870e40ae0, ; 761: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 200
	i64 u0xfd49b3c1a76e2748, ; 762: System.Runtime.InteropServices.RuntimeInformation => 109
	i64 u0xfd536c702f64dc47, ; 763: System.Text.Encoding.Extensions => 137
	i64 u0xfd583f7657b6a1cb, ; 764: Xamarin.AndroidX.Fragment => 204
	i64 u0xfd8dd91a2c26bd5d, ; 765: Xamarin.AndroidX.Lifecycle.Runtime => 212
	i64 u0xfda36abccf05cf5c, ; 766: System.Net.WebSockets.Client => 82
	i64 u0xfddbe9695626a7f5, ; 767: Xamarin.AndroidX.Lifecycle.Common => 208
	i64 u0xff270a55858bac8d, ; 768: System.Security.Principal => 131
	i64 u0xff9b54613e0d2cc8, ; 769: System.Net.Http.Json => 65
	i64 u0xffdb7a971be4ec73 ; 770: System.ValueTuple.dll => 155
], align 16

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [771 x i32] [
	i32 42, i32 248, i32 13, i32 206, i32 107, i32 174, i32 48, i32 183,
	i32 7, i32 88, i32 201, i32 72, i32 225, i32 12, i32 104, i32 159,
	i32 19, i32 189, i32 164, i32 203, i32 234, i32 170, i32 10, i32 235,
	i32 98, i32 199, i32 200, i32 13, i32 10, i32 129, i32 97, i32 143,
	i32 39, i32 251, i32 237, i32 175, i32 5, i32 68, i32 132, i32 229,
	i32 202, i32 69, i32 67, i32 0, i32 206, i32 57, i32 198, i32 52,
	i32 43, i32 127, i32 68, i32 83, i32 214, i32 254, i32 161, i32 94,
	i32 101, i32 225, i32 144, i32 154, i32 165, i32 172, i32 83, i32 254,
	i32 243, i32 4, i32 5, i32 51, i32 103, i32 56, i32 122, i32 100,
	i32 171, i32 120, i32 248, i32 21, i32 139, i32 99, i32 251, i32 79,
	i32 230, i32 121, i32 8, i32 168, i32 71, i32 215, i32 226, i32 174,
	i32 148, i32 40, i32 47, i32 30, i32 147, i32 166, i32 28, i32 86,
	i32 231, i32 79, i32 43, i32 29, i32 42, i32 105, i32 119, i32 181,
	i32 45, i32 93, i32 56, i32 151, i32 253, i32 149, i32 102, i32 49,
	i32 20, i32 194, i32 116, i32 244, i32 96, i32 58, i32 83, i32 172,
	i32 26, i32 72, i32 224, i32 252, i32 70, i32 33, i32 14, i32 141,
	i32 38, i32 190, i32 136, i32 94, i32 90, i32 152, i32 24, i32 140,
	i32 57, i32 142, i32 51, i32 29, i32 160, i32 34, i32 167, i32 204,
	i32 52, i32 256, i32 92, i32 187, i32 205, i32 35, i32 160, i32 9,
	i32 78, i32 59, i32 55, i32 13, i32 238, i32 185, i32 111, i32 218,
	i32 32, i32 106, i32 86, i32 94, i32 53, i32 98, i32 242, i32 58,
	i32 0, i32 9, i32 104, i32 198, i32 69, i32 237, i32 127, i32 226,
	i32 118, i32 137, i32 228, i32 128, i32 108, i32 244, i32 133, i32 241,
	i32 150, i32 159, i32 194, i32 201, i32 226, i32 99, i32 24, i32 146,
	i32 3, i32 170, i32 184, i32 102, i32 164, i32 101, i32 196, i32 25,
	i32 206, i32 95, i32 171, i32 175, i32 179, i32 3, i32 232, i32 203,
	i32 1, i32 116, i32 244, i32 211, i32 33, i32 6, i32 205, i32 159,
	i32 53, i32 87, i32 236, i32 44, i32 106, i32 47, i32 140, i32 220,
	i32 65, i32 221, i32 70, i32 82, i32 60, i32 91, i32 157, i32 185,
	i32 135, i32 112, i32 59, i32 221, i32 224, i32 174, i32 136, i32 143,
	i32 40, i32 61, i32 217, i32 81, i32 25, i32 36, i32 101, i32 214,
	i32 72, i32 22, i32 194, i32 123, i32 70, i32 109, i32 121, i32 119,
	i32 208, i32 209, i32 11, i32 2, i32 126, i32 117, i32 145, i32 41,
	i32 89, i32 180, i32 177, i32 27, i32 151, i32 240, i32 179, i32 1,
	i32 181, i32 44, i32 193, i32 152, i32 18, i32 88, i32 41, i32 188,
	i32 215, i32 96, i32 28, i32 41, i32 80, i32 202, i32 190, i32 147,
	i32 110, i32 189, i32 11, i32 107, i32 139, i32 16, i32 124, i32 67,
	i32 160, i32 22, i32 250, i32 104, i32 249, i32 64, i32 58, i32 112,
	i32 177, i32 255, i32 247, i32 9, i32 239, i32 122, i32 100, i32 107,
	i32 76, i32 218, i32 113, i32 182, i32 49, i32 59, i32 20, i32 217,
	i32 197, i32 73, i32 192, i32 158, i32 39, i32 35, i32 245, i32 38,
	i32 110, i32 21, i32 242, i32 216, i32 15, i32 81, i32 81, i32 197,
	i32 229, i32 155, i32 21, i32 50, i32 51, i32 96, i32 16, i32 196,
	i32 125, i32 163, i32 45, i32 240, i32 118, i32 64, i32 223, i32 169,
	i32 14, i32 227, i32 113, i32 182, i32 61, i32 246, i32 76, i32 123,
	i32 2, i32 204, i32 216, i32 245, i32 243, i32 216, i32 6, i32 188,
	i32 201, i32 17, i32 79, i32 191, i32 133, i32 242, i32 85, i32 12,
	i32 34, i32 121, i32 250, i32 211, i32 176, i32 203, i32 87, i32 18,
	i32 237, i32 210, i32 73, i32 253, i32 97, i32 168, i32 207, i32 84,
	i32 183, i32 187, i32 246, i32 157, i32 36, i32 154, i32 147, i32 56,
	i32 115, i32 188, i32 234, i32 233, i32 37, i32 117, i32 0, i32 181,
	i32 14, i32 149, i32 43, i32 179, i32 100, i32 249, i32 171, i32 16,
	i32 228, i32 48, i32 109, i32 99, i32 221, i32 27, i32 130, i32 29,
	i32 229, i32 130, i32 44, i32 197, i32 202, i32 152, i32 8, i32 232,
	i32 134, i32 42, i32 250, i32 33, i32 256, i32 46, i32 146, i32 217,
	i32 198, i32 140, i32 63, i32 134, i32 48, i32 163, i32 186, i32 208,
	i32 233, i32 46, i32 167, i32 176, i32 207, i32 18, i32 8, i32 195,
	i32 228, i32 126, i32 60, i32 144, i32 212, i32 153, i32 145, i32 248,
	i32 245, i32 128, i32 247, i32 163, i32 165, i32 199, i32 178, i32 26,
	i32 210, i32 84, i32 129, i32 103, i32 151, i32 239, i32 54, i32 165,
	i32 170, i32 133, i32 37, i32 235, i32 22, i32 114, i32 92, i32 50,
	i32 61, i32 124, i32 85, i32 129, i32 166, i32 169, i32 227, i32 200,
	i32 213, i32 4, i32 209, i32 173, i32 2, i32 218, i32 118, i32 255,
	i32 180, i32 19, i32 91, i32 66, i32 30, i32 205, i32 192, i32 60,
	i32 113, i32 210, i32 32, i32 130, i32 162, i32 190, i32 143, i32 156,
	i32 17, i32 77, i32 75, i32 15, i32 172, i32 87, i32 246, i32 126,
	i32 219, i32 191, i32 215, i32 34, i32 120, i32 141, i32 124, i32 108,
	i32 253, i32 222, i32 235, i32 187, i32 54, i32 47, i32 28, i32 142,
	i32 148, i32 150, i32 35, i32 177, i32 77, i32 164, i32 1, i32 220,
	i32 162, i32 12, i32 158, i32 154, i32 78, i32 105, i32 114, i32 185,
	i32 66, i32 67, i32 238, i32 45, i32 111, i32 7, i32 184, i32 55,
	i32 180, i32 65, i32 195, i32 20, i32 111, i32 103, i32 63, i32 145,
	i32 178, i32 7, i32 173, i32 50, i32 238, i32 117, i32 144, i32 169,
	i32 82, i32 115, i32 219, i32 17, i32 74, i32 91, i32 89, i32 122,
	i32 231, i32 182, i32 137, i32 156, i32 108, i32 11, i32 92, i32 31,
	i32 138, i32 230, i32 40, i32 256, i32 141, i32 249, i32 251, i32 25,
	i32 176, i32 74, i32 207, i32 255, i32 27, i32 68, i32 90, i32 97,
	i32 115, i32 31, i32 106, i32 209, i32 37, i32 73, i32 241, i32 110,
	i32 125, i32 184, i32 89, i32 88, i32 95, i32 131, i32 219, i32 231,
	i32 252, i32 227, i32 195, i32 192, i32 223, i32 166, i32 132, i32 236,
	i32 224, i32 10, i32 49, i32 93, i32 153, i32 63, i32 138, i32 153,
	i32 62, i32 119, i32 139, i32 243, i32 86, i32 162, i32 233, i32 146,
	i32 84, i32 76, i32 71, i32 183, i32 138, i32 193, i32 127, i32 252,
	i32 54, i32 112, i32 132, i32 90, i32 23, i32 75, i32 131, i32 31,
	i32 74, i32 214, i32 161, i32 23, i32 4, i32 173, i32 125, i32 196,
	i32 222, i32 116, i32 175, i32 32, i32 3, i32 167, i32 234, i32 30,
	i32 19, i32 223, i32 213, i32 95, i32 36, i32 5, i32 186, i32 247,
	i32 158, i32 230, i32 240, i32 191, i32 236, i32 254, i32 78, i32 64,
	i32 150, i32 189, i32 123, i32 136, i32 232, i32 241, i32 102, i32 39,
	i32 178, i32 142, i32 69, i32 26, i32 77, i32 80, i32 212, i32 24,
	i32 155, i32 38, i32 220, i32 186, i32 135, i32 222, i32 105, i32 239,
	i32 57, i32 168, i32 93, i32 62, i32 134, i32 46, i32 135, i32 199,
	i32 148, i32 80, i32 193, i32 213, i32 157, i32 85, i32 62, i32 98,
	i32 225, i32 156, i32 120, i32 6, i32 15, i32 75, i32 149, i32 52,
	i32 71, i32 23, i32 161, i32 128, i32 66, i32 114, i32 211, i32 55,
	i32 53, i32 200, i32 109, i32 137, i32 204, i32 212, i32 82, i32 208,
	i32 131, i32 65, i32 155
], align 16

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.mm.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.mm.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 16

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+crc32,+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }

; Metadata
!llvm.module.flags = !{!0, !1}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/10.0.1xx @ d549e1dc4e2a083b08b4f24cb5495e81b99d79b5"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
