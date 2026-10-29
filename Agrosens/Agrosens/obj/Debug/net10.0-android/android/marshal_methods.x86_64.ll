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

@assembly_image_cache = dso_local local_unnamed_addr global [262 x ptr] zeroinitializer, align 16

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [786 x i64] [
	i64 u0x001e58127c546039, ; 0: lib_System.Globalization.dll.so => 42
	i64 u0x0024d0f62dee05bd, ; 1: Xamarin.KotlinX.Coroutines.Core.dll => 253
	i64 u0x01109b0e4d99e61f, ; 2: System.ComponentModel.Annotations.dll => 13
	i64 u0x01a68c7fd2565067, ; 3: Xamarin.AndroidX.Graphics.Shapes.Android => 211
	i64 u0x0284512fad379f7e, ; 4: System.Runtime.Handles => 107
	i64 u0x02abedc11addc1ed, ; 5: lib_Mono.Android.Runtime.dll.so => 174
	i64 u0x02f55bf70672f5c8, ; 6: lib_System.IO.FileSystem.DriveInfo.dll.so => 48
	i64 u0x032267b2a94db371, ; 7: lib_Xamarin.AndroidX.AppCompat.dll.so => 188
	i64 u0x03621c804933a890, ; 8: System.Buffers => 7
	i64 u0x0363ac97a4cb84e6, ; 9: SQLitePCLRaw.provider.e_sqlite3.dll => 182
	i64 u0x0399610510a38a38, ; 10: lib_System.Private.DataContractSerialization.dll.so => 88
	i64 u0x047408741db2431a, ; 11: Xamarin.AndroidX.DynamicAnimation => 206
	i64 u0x0517ef04e06e9f76, ; 12: System.Net.Primitives => 72
	i64 u0x0565d18c6da3de38, ; 13: Xamarin.AndroidX.RecyclerView => 230
	i64 u0x0581db89237110e9, ; 14: lib_System.Collections.dll.so => 12
	i64 u0x05a1c25e78e22d87, ; 15: lib_System.Runtime.CompilerServices.Unsafe.dll.so => 104
	i64 u0x06388ffe9f6c161a, ; 16: System.Xml.Linq.dll => 159
	i64 u0x06600c4c124cb358, ; 17: System.Configuration.dll => 19
	i64 u0x0680a433c781bb3d, ; 18: Xamarin.AndroidX.Collection.Jvm => 194
	i64 u0x069fff96ec92a91d, ; 19: System.Xml.XPath.dll => 164
	i64 u0x070b0847e18dab68, ; 20: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 208
	i64 u0x0739448d84d3b016, ; 21: lib_Xamarin.AndroidX.VectorDrawable.dll.so => 239
	i64 u0x07469f2eecce9e85, ; 22: mscorlib.dll => 170
	i64 u0x07dcdc7460a0c5e4, ; 23: System.Collections.NonGeneric => 10
	i64 u0x088610fc2509f69e, ; 24: lib_Xamarin.AndroidX.VectorDrawable.Animated.dll.so => 240
	i64 u0x08a7c865576bbde7, ; 25: System.Reflection.Primitives => 98
	i64 u0x08c9d051a4a817e5, ; 26: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 204
	i64 u0x08f3c9788ee2153c, ; 27: Xamarin.AndroidX.DrawerLayout => 205
	i64 u0x09138715c92dba90, ; 28: lib_System.ComponentModel.Annotations.dll.so => 13
	i64 u0x092266563089ae3e, ; 29: lib_System.Collections.NonGeneric.dll.so => 10
	i64 u0x09d144a7e214d457, ; 30: System.Security.Cryptography => 129
	i64 u0x09e2b9f743db21a8, ; 31: lib_System.Reflection.Metadata.dll.so => 97
	i64 u0x0abb3e2b271edc45, ; 32: System.Threading.Channels.dll => 143
	i64 u0x0b06b1feab070143, ; 33: System.Formats.Tar => 39
	i64 u0x0b6aff547b84fbe9, ; 34: Xamarin.KotlinX.Serialization.Core.Jvm => 256
	i64 u0x0be2e1f8ce4064ed, ; 35: Xamarin.AndroidX.ViewPager => 242
	i64 u0x0c59ad9fbbd43abe, ; 36: Mono.Android => 175
	i64 u0x0c74af560004e816, ; 37: Microsoft.Win32.Registry.dll => 5
	i64 u0x0c83c82812e96127, ; 38: lib_System.Net.Mail.dll.so => 68
	i64 u0x0d13cd7cce4284e4, ; 39: System.Security.SecureString => 132
	i64 u0x0d63f4f73521c24f, ; 40: lib_Xamarin.AndroidX.SavedState.SavedState.Ktx.dll.so => 234
	i64 u0x0e04e702012f8463, ; 41: Xamarin.AndroidX.Emoji2 => 207
	i64 u0x0e14e73a54dda68e, ; 42: lib_System.Net.NameResolution.dll.so => 69
	i64 u0x0f5e7abaa7cf470a, ; 43: System.Net.HttpListener => 67
	i64 u0x0fb42772d86bc9a0, ; 44: Agrosens => 0
	i64 u0x0ff2f230edd25a94, ; 45: lib_Xamarin.AndroidX.Graphics.Shapes.Android.dll.so => 211
	i64 u0x1001f97bbe242e64, ; 46: System.IO.UnmanagedMemoryStream => 57
	i64 u0x102a31b45304b1da, ; 47: Xamarin.AndroidX.CustomView => 203
	i64 u0x1065c4cb554c3d75, ; 48: System.IO.IsolatedStorage.dll => 52
	i64 u0x10f6cfcbcf801616, ; 49: System.IO.Compression.Brotli => 43
	i64 u0x114443cdcf2091f1, ; 50: System.Security.Cryptography.Primitives => 127
	i64 u0x11a603952763e1d4, ; 51: System.Net.Mail => 68
	i64 u0x11a70d0e1009fb11, ; 52: System.Net.WebSockets.dll => 83
	i64 u0x11f26371eee0d3c1, ; 53: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll.so => 219
	i64 u0x11fbe62d469cc1c8, ; 54: Microsoft.VisualStudio.DesignTools.TapContract.dll => 259
	i64 u0x12128b3f59302d47, ; 55: lib_System.Xml.Serialization.dll.so => 161
	i64 u0x123639456fb056da, ; 56: System.Reflection.Emit.Lightweight.dll => 94
	i64 u0x12521e9764603eaa, ; 57: lib_System.Resources.Reader.dll.so => 101
	i64 u0x125b7f94acb989db, ; 58: Xamarin.AndroidX.RecyclerView.dll => 230
	i64 u0x12d3b63863d4ab0b, ; 59: lib_System.Threading.Overlapped.dll.so => 144
	i64 u0x134eab1061c395ee, ; 60: System.Transactions => 154
	i64 u0x13beedefb0e28a45, ; 61: lib_System.Xml.XmlDocument.dll.so => 165
	i64 u0x13f1e5e209e91af4, ; 62: lib_Java.Interop.dll.so => 172
	i64 u0x1497051b917530bd, ; 63: lib_System.Net.WebSockets.dll.so => 83
	i64 u0x14b78ce3adce0011, ; 64: Microsoft.VisualStudio.DesignTools.TapContract => 259
	i64 u0x14d612a531c79c05, ; 65: Xamarin.JSpecify.dll => 248
	i64 u0x152a448bd1e745a7, ; 66: Microsoft.Win32.Primitives => 4
	i64 u0x1557de0138c445f4, ; 67: lib_Microsoft.Win32.Registry.dll.so => 5
	i64 u0x15bdc156ed462f2f, ; 68: lib_System.IO.FileSystem.dll.so => 51
	i64 u0x15e300c2c1668655, ; 69: System.Resources.Writer.dll => 103
	i64 u0x16bf2a22df043a09, ; 70: System.IO.Pipes.dll => 56
	i64 u0x16ea2b318ad2d830, ; 71: System.Security.Cryptography.Algorithms => 122
	i64 u0x16eeae54c7ebcc08, ; 72: System.Reflection.dll => 100
	i64 u0x17125c9a85b4929f, ; 73: lib_netstandard.dll.so => 171
	i64 u0x1716866f7416792e, ; 74: lib_System.Security.AccessControl.dll.so => 120
	i64 u0x174f71c46216e44a, ; 75: Xamarin.KotlinX.Coroutines.Core => 253
	i64 u0x1752c12f1e1fc00c, ; 76: System.Core => 21
	i64 u0x17f9358913beb16a, ; 77: System.Text.Encodings.Web => 139
	i64 u0x1809fb23f29ba44a, ; 78: lib_System.Reflection.TypeExtensions.dll.so => 99
	i64 u0x18402a709e357f3b, ; 79: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 256
	i64 u0x18a9befae51bb361, ; 80: System.Net.WebClient => 79
	i64 u0x19777fba3c41b398, ; 81: Xamarin.AndroidX.Startup.StartupRuntime.dll => 235
	i64 u0x19a4c090f14ebb66, ; 82: System.Security.Claims => 121
	i64 u0x1a91866a319e9259, ; 83: lib_System.Collections.Concurrent.dll.so => 8
	i64 u0x1aac34d1917ba5d3, ; 84: lib_System.dll.so => 168
	i64 u0x1aea8f1c3b282172, ; 85: lib_System.Net.Ping.dll.so => 71
	i64 u0x1bbdb16cfa73e785, ; 86: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android => 220
	i64 u0x1bc766e07b2b4241, ; 87: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 231
	i64 u0x1c753b5ff15bce1b, ; 88: Mono.Android.Runtime.dll => 174
	i64 u0x1cd47467799d8250, ; 89: System.Threading.Tasks.dll => 148
	i64 u0x1d23eafdc6dc346c, ; 90: System.Globalization.Calendars.dll => 40
	i64 u0x1db6820994506bf5, ; 91: System.IO.FileSystem.AccessControl.dll => 47
	i64 u0x1dbb0c2c6a999acb, ; 92: System.Diagnostics.StackTrace => 30
	i64 u0x1e7c31185e2fb266, ; 93: lib_System.Threading.Tasks.Parallel.dll.so => 147
	i64 u0x1f055d15d807e1b2, ; 94: System.Xml.XmlSerializer => 166
	i64 u0x1f1ed22c1085f044, ; 95: lib_System.Diagnostics.FileVersionInfo.dll.so => 28
	i64 u0x1f61df9c5b94d2c1, ; 96: lib_System.Numerics.dll.so => 86
	i64 u0x1f750bb5421397de, ; 97: lib_Xamarin.AndroidX.Tracing.Tracing.dll.so => 236
	i64 u0x20237ea48006d7a8, ; 98: lib_System.Net.WebClient.dll.so => 79
	i64 u0x209375905fcc1bad, ; 99: lib_System.IO.Compression.Brotli.dll.so => 43
	i64 u0x20fab3cf2dfbc8df, ; 100: lib_System.Diagnostics.Process.dll.so => 29
	i64 u0x2110167c128cba15, ; 101: System.Globalization => 42
	i64 u0x21419508838f7547, ; 102: System.Runtime.CompilerServices.VisualC => 105
	i64 u0x2174319c0d835bc9, ; 103: System.Runtime => 119
	i64 u0x2198e5bc8b7153fa, ; 104: Xamarin.AndroidX.Annotation.Experimental.dll => 186
	i64 u0x219ea1b751a4dee4, ; 105: lib_System.IO.Compression.ZipFile.dll.so => 45
	i64 u0x21cc7e445dcd5469, ; 106: System.Reflection.Emit.ILGeneration => 93
	i64 u0x224538d85ed15a82, ; 107: System.IO.Pipes => 56
	i64 u0x22908438c6bed1af, ; 108: lib_System.Threading.Timer.dll.so => 151
	i64 u0x22fbc14e981e3b45, ; 109: lib_Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll.so => 258
	i64 u0x237be844f1f812c7, ; 110: System.Threading.Thread.dll => 149
	i64 u0x23852b3bdc9f7096, ; 111: System.Resources.ResourceManager => 102
	i64 u0x23986dd7e5d4fc01, ; 112: System.IO.FileSystem.Primitives.dll => 49
	i64 u0x2407aef2bbe8fadf, ; 113: System.Console => 20
	i64 u0x240abe014b27e7d3, ; 114: Xamarin.AndroidX.Core.dll => 199
	i64 u0x247619fe4413f8bf, ; 115: System.Runtime.Serialization.Primitives.dll => 116
	i64 u0x25a0a7eff76ea08e, ; 116: SQLitePCLRaw.batteries_v2.dll => 179
	i64 u0x2662c629b96b0b30, ; 117: lib_Xamarin.Kotlin.StdLib.dll.so => 249
	i64 u0x26a670e154a9c54b, ; 118: System.Reflection.Extensions.dll => 96
	i64 u0x26d077d9678fe34f, ; 119: System.IO.dll => 58
	i64 u0x2759af78ab94d39b, ; 120: System.Net.WebSockets => 83
	i64 u0x27b410442fad6cf1, ; 121: Java.Interop.dll => 172
	i64 u0x27b97e0d52c3034a, ; 122: System.Diagnostics.Debug => 26
	i64 u0x2801845a2c71fbfb, ; 123: System.Net.Primitives.dll => 72
	i64 u0x286835e259162700, ; 124: lib_Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll.so => 229
	i64 u0x29f947844fb7fc11, ; 125: Microsoft.Maui.Controls.HotReload.Forms => 257
	i64 u0x2a3b095612184159, ; 126: lib_System.Net.NetworkInformation.dll.so => 70
	i64 u0x2a6507a5ffabdf28, ; 127: System.Diagnostics.TraceSource.dll => 33
	i64 u0x2ad5d6b13b7a3e04, ; 128: System.ComponentModel.DataAnnotations.dll => 14
	i64 u0x2af298f63581d886, ; 129: System.Text.RegularExpressions.dll => 141
	i64 u0x2afc1c4f898552ee, ; 130: lib_System.Formats.Asn1.dll.so => 38
	i64 u0x2b6989d78cba9a15, ; 131: Xamarin.AndroidX.Concurrent.Futures.dll => 195
	i64 u0x2cbd9262ca785540, ; 132: lib_System.Text.Encoding.CodePages.dll.so => 136
	i64 u0x2cc9e1fed6257257, ; 133: lib_System.Reflection.Emit.Lightweight.dll.so => 94
	i64 u0x2cd723e9fe623c7c, ; 134: lib_System.Private.Xml.Linq.dll.so => 90
	i64 u0x2d169d318a968379, ; 135: System.Threading.dll => 152
	i64 u0x2d5ffcae1ad0aaca, ; 136: System.Data.dll => 24
	i64 u0x2db915caf23548d2, ; 137: System.Text.Json.dll => 140
	i64 u0x2dcaa0bb15a4117a, ; 138: System.IO.UnmanagedMemoryStream.dll => 57
	i64 u0x2e2ced2c3c6a1edc, ; 139: lib_System.Threading.AccessControl.dll.so => 142
	i64 u0x2e5a40c319acb800, ; 140: System.IO.FileSystem => 51
	i64 u0x2f02f94df3200fe5, ; 141: System.Diagnostics.Process => 29
	i64 u0x2f2e98e1c89b1aff, ; 142: System.Xml.ReaderWriter => 160
	i64 u0x2f5911d9ba814e4e, ; 143: System.Diagnostics.Tracing => 34
	i64 u0x2f84070a459bc31f, ; 144: lib_System.Xml.dll.so => 167
	i64 u0x309ee9eeec09a71e, ; 145: lib_Xamarin.AndroidX.Fragment.dll.so => 209
	i64 u0x30c6dda129408828, ; 146: System.IO.IsolatedStorage => 52
	i64 u0x31195fef5d8fb552, ; 147: _Microsoft.Android.Resource.Designer.dll => 261
	i64 u0x31496b779ed0663d, ; 148: lib_System.Reflection.DispatchProxy.dll.so => 92
	i64 u0x32243413e774362a, ; 149: Xamarin.AndroidX.CardView.dll => 192
	i64 u0x32319d9df33b1594, ; 150: lib_Xamarin.AndroidX.Graphics.Shapes.dll.so => 210
	i64 u0x3235427f8d12dae1, ; 151: lib_System.Drawing.Primitives.dll.so => 35
	i64 u0x32aa989ff07a84ff, ; 152: lib_System.Xml.ReaderWriter.dll.so => 160
	i64 u0x33829542f112d59b, ; 153: System.Collections.Immutable => 9
	i64 u0x341abc357fbb4ebf, ; 154: lib_System.Net.Sockets.dll.so => 78
	i64 u0x346a212343615ac5, ; 155: lib_System.Linq.AsyncEnumerable.dll.so => 59
	i64 u0x3496c1e2dcaf5ecc, ; 156: lib_System.IO.Pipes.AccessControl.dll.so => 55
	i64 u0x353590da528c9d22, ; 157: System.ComponentModel.Annotations => 13
	i64 u0x3549870798b4cd30, ; 158: lib_Xamarin.AndroidX.ViewPager2.dll.so => 243
	i64 u0x3552fc5d578f0fbf, ; 159: Xamarin.AndroidX.Arch.Core.Common => 190
	i64 u0x355c649948d55d97, ; 160: lib_System.Runtime.Intrinsics.dll.so => 111
	i64 u0x35ea9d1c6834bc8c, ; 161: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 223
	i64 u0x3628ab68db23a01a, ; 162: lib_System.Diagnostics.Tools.dll.so => 32
	i64 u0x3673b042508f5b6b, ; 163: lib_System.Runtime.Extensions.dll.so => 106
	i64 u0x36740f1a8ecdc6c4, ; 164: System.Numerics => 86
	i64 u0x36b2b50fdf589ae2, ; 165: System.Reflection.Emit.Lightweight => 94
	i64 u0x36cada77dc79928b, ; 166: System.IO.MemoryMappedFiles => 53
	i64 u0x374ef46b06791af6, ; 167: System.Reflection.Primitives.dll => 98
	i64 u0x376bf93e521a5417, ; 168: lib_Xamarin.Jetbrains.Annotations.dll.so => 247
	i64 u0x37bc29f3183003b6, ; 169: lib_System.IO.dll.so => 58
	i64 u0x37f55a2ad394d0c1, ; 170: Agrosens.dll => 0
	i64 u0x380134e03b1e160a, ; 171: System.Collections.Immutable.dll => 9
	i64 u0x38049b5c59b39324, ; 172: System.Runtime.CompilerServices.Unsafe => 104
	i64 u0x385c17636bb6fe6e, ; 173: Xamarin.AndroidX.CustomView.dll => 203
	i64 u0x38869c811d74050e, ; 174: System.Net.NameResolution.dll => 69
	i64 u0x393c226616977fdb, ; 175: lib_Xamarin.AndroidX.ViewPager.dll.so => 242
	i64 u0x3ab5859054645f72, ; 176: System.Security.Cryptography.Primitives.dll => 127
	i64 u0x3ad75090c3fac0e9, ; 177: lib_Xamarin.AndroidX.ResourceInspection.Annotation.dll.so => 231
	i64 u0x3ae44ac43a1fbdbb, ; 178: System.Runtime.Serialization => 118
	i64 u0x3b860f9932505633, ; 179: lib_System.Text.Encoding.Extensions.dll.so => 137
	i64 u0x3be99b43dd39dd37, ; 180: Xamarin.AndroidX.SavedState.SavedState.Android => 233
	i64 u0x3c3aafb6b3a00bf6, ; 181: lib_System.Security.Cryptography.X509Certificates.dll.so => 128
	i64 u0x3c4049146b59aa90, ; 182: System.Runtime.InteropServices.JavaScript => 108
	i64 u0x3c7c495f58ac5ee9, ; 183: Xamarin.Kotlin.StdLib => 249
	i64 u0x3c7e5ed3d5db71bb, ; 184: System.Security => 133
	i64 u0x3d1c50cc001a991e, ; 185: Xamarin.Google.Guava.ListenableFuture.dll => 246
	i64 u0x3d2b1913edfc08d7, ; 186: lib_System.Threading.ThreadPool.dll.so => 150
	i64 u0x3d46f0b995082740, ; 187: System.Xml.Linq => 159
	i64 u0x3d9c2a242b040a50, ; 188: lib_Xamarin.AndroidX.Core.dll.so => 199
	i64 u0x3da7781d6333a8fe, ; 189: SQLitePCLRaw.batteries_v2 => 179
	i64 u0x3dbb6b9f5ab90fa7, ; 190: lib_Xamarin.AndroidX.DynamicAnimation.dll.so => 206
	i64 u0x3e5441657549b213, ; 191: Xamarin.AndroidX.ResourceInspection.Annotation => 231
	i64 u0x3e57d4d195c53c2e, ; 192: System.Reflection.TypeExtensions => 99
	i64 u0x3e616ab4ed1f3f15, ; 193: lib_System.Data.dll.so => 24
	i64 u0x3f510adf788828dd, ; 194: System.Threading.Tasks.Extensions => 146
	i64 u0x40c98b6bd77346d4, ; 195: Microsoft.VisualBasic.dll => 3
	i64 u0x41833cf766d27d96, ; 196: mscorlib => 170
	i64 u0x41cab042be111c34, ; 197: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 189
	i64 u0x423a9ecc4d905a88, ; 198: lib_System.Resources.ResourceManager.dll.so => 102
	i64 u0x423bf51ae7def810, ; 199: System.Xml.XPath => 164
	i64 u0x42462ff15ddba223, ; 200: System.Resources.Reader.dll => 101
	i64 u0x4291015ff4e5ef71, ; 201: Xamarin.AndroidX.Core.ViewTree.dll => 201
	i64 u0x42a31b86e6ccc3f0, ; 202: System.Diagnostics.Contracts => 25
	i64 u0x42b32a3d24365ec8, ; 203: Xamarin.AndroidX.Graphics.Shapes.Android.dll => 211
	i64 u0x430e95b891249788, ; 204: lib_System.Reflection.Emit.dll.so => 95
	i64 u0x43375950ec7c1b6a, ; 205: netstandard.dll => 171
	i64 u0x434c4e1d9284cdae, ; 206: Mono.Android.dll => 175
	i64 u0x43505013578652a0, ; 207: lib_Xamarin.AndroidX.Activity.Ktx.dll.so => 184
	i64 u0x437d06c381ed575a, ; 208: lib_Microsoft.VisualBasic.dll.so => 3
	i64 u0x43c077442b230f64, ; 209: Xamarin.AndroidX.Tracing.Tracing.Android => 237
	i64 u0x43e8ca5bc927ff37, ; 210: lib_Xamarin.AndroidX.Emoji2.ViewsHelper.dll.so => 208
	i64 u0x448bd33429269b19, ; 211: Microsoft.CSharp => 1
	i64 u0x4499fa3c8e494654, ; 212: lib_System.Runtime.Serialization.Primitives.dll.so => 116
	i64 u0x4515080865a951a5, ; 213: Xamarin.Kotlin.StdLib.dll => 249
	i64 u0x454b4d1e66bb783c, ; 214: Xamarin.AndroidX.Lifecycle.Process => 216
	i64 u0x45c40276a42e283e, ; 215: System.Diagnostics.TraceSource => 33
	i64 u0x45d443f2a29adc37, ; 216: System.AppContext.dll => 6
	i64 u0x46b58f11aea9aa05, ; 217: Xamarin.AndroidX.Graphics.Shapes.dll => 210
	i64 u0x47358bd471172e1d, ; 218: lib_System.Xml.Linq.dll.so => 159
	i64 u0x480c0a47dd42dd81, ; 219: lib_System.IO.MemoryMappedFiles.dll.so => 53
	i64 u0x49e952f19a4e2022, ; 220: System.ObjectModel => 87
	i64 u0x49f9e6948a8131e4, ; 221: lib_Xamarin.AndroidX.VersionedParcelable.dll.so => 241
	i64 u0x4a7a18981dbd56bc, ; 222: System.IO.Compression.FileSystem.dll => 44
	i64 u0x4b07a0ed0ab33ff4, ; 223: System.Runtime.Extensions.dll => 106
	i64 u0x4b576d47ac054f3c, ; 224: System.IO.FileSystem.AccessControl => 47
	i64 u0x4b7b6532ded934b7, ; 225: System.Text.Json => 140
	i64 u0x4c2029a97af23a8d, ; 226: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.Android => 225
	i64 u0x4c7755cf07ad2d5f, ; 227: System.Net.Http.Json.dll => 65
	i64 u0x4cc5f15266470798, ; 228: lib_Xamarin.AndroidX.Loader.dll.so => 226
	i64 u0x4cf6f67dc77aacd2, ; 229: System.Net.NetworkInformation.dll => 70
	i64 u0x4d3183dd245425d4, ; 230: System.Net.WebSockets.Client.dll => 82
	i64 u0x4d479f968a05e504, ; 231: System.Linq.Expressions.dll => 60
	i64 u0x4d55a010ffc4faff, ; 232: System.Private.Xml => 91
	i64 u0x4d5cbe77561c5b2e, ; 233: System.Web.dll => 157
	i64 u0x4d77512dbd86ee4c, ; 234: lib_Xamarin.AndroidX.Arch.Core.Common.dll.so => 190
	i64 u0x4d7793536e79c309, ; 235: System.ServiceProcess => 135
	i64 u0x4d95fccc1f67c7ca, ; 236: System.Runtime.Loader.dll => 112
	i64 u0x4db014bf0ff1c9c1, ; 237: System.Linq.AsyncEnumerable => 59
	i64 u0x4dd9247f1d2c3235, ; 238: Xamarin.AndroidX.Loader.dll => 226
	i64 u0x4e2aeee78e2c4a87, ; 239: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 229
	i64 u0x4e32f00cb0937401, ; 240: Mono.Android.Runtime => 174
	i64 u0x4e5eea4668ac2b18, ; 241: System.Text.Encoding.CodePages => 136
	i64 u0x4ebd0c4b82c5eefc, ; 242: lib_System.Threading.Channels.dll.so => 143
	i64 u0x4ee8eaa9c9c1151a, ; 243: System.Globalization.Calendars => 40
	i64 u0x4fd5f3ee53d0a4f0, ; 244: SQLitePCLRaw.lib.e_sqlite3.android => 181
	i64 u0x50c3a29b21050d45, ; 245: System.Linq.Parallel.dll => 61
	i64 u0x5131bbe80989093f, ; 246: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 222
	i64 u0x516324a5050a7e3c, ; 247: System.Net.WebProxy => 81
	i64 u0x516d6f0b21a303de, ; 248: lib_System.Diagnostics.Contracts.dll.so => 25
	i64 u0x51bb8a2afe774e32, ; 249: System.Drawing => 36
	i64 u0x5247c5c32a4140f0, ; 250: System.Resources.Reader => 101
	i64 u0x526bb15e3c386364, ; 251: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 219
	i64 u0x526ce79eb8e90527, ; 252: lib_System.Net.Primitives.dll.so => 72
	i64 u0x52829f00b4467c38, ; 253: lib_System.Data.Common.dll.so => 22
	i64 u0x529ffe06f39ab8db, ; 254: Xamarin.AndroidX.Core => 199
	i64 u0x53978aac584c666e, ; 255: lib_System.Security.Cryptography.Cng.dll.so => 123
	i64 u0x53a96d5c86c9e194, ; 256: System.Net.NetworkInformation => 70
	i64 u0x53be1038a61e8d44, ; 257: System.Runtime.InteropServices.RuntimeInformation.dll => 109
	i64 u0x5435e6f049e9bc37, ; 258: System.Security.Claims.dll => 121
	i64 u0x54795225dd1587af, ; 259: lib_System.Runtime.dll.so => 119
	i64 u0x547a34f14e5f6210, ; 260: Xamarin.AndroidX.Lifecycle.Common.dll => 213
	i64 u0x556e8b63b660ab8b, ; 261: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 214
	i64 u0x5588627c9a108ec9, ; 262: System.Collections.Specialized => 11
	i64 u0x55a898e4f42e3fae, ; 263: Microsoft.VisualBasic.Core.dll => 2
	i64 u0x55fa0c610fe93bb1, ; 264: lib_System.Security.Cryptography.OpenSsl.dll.so => 126
	i64 u0x56442b99bc64bb47, ; 265: System.Runtime.Serialization.Xml.dll => 117
	i64 u0x56a8b26e1aeae27b, ; 266: System.Threading.Tasks.Dataflow => 145
	i64 u0x56f932d61e93c07f, ; 267: System.Globalization.Extensions => 41
	i64 u0x571c5cfbec5ae8e2, ; 268: System.Private.Uri => 89
	i64 u0x576499c9f52fea31, ; 269: Xamarin.AndroidX.Annotation => 185
	i64 u0x578cd35c91d7b347, ; 270: lib_SQLitePCLRaw.core.dll.so => 180
	i64 u0x579a06fed6eec900, ; 271: System.Private.CoreLib.dll => 177
	i64 u0x57c542c14049b66d, ; 272: System.Diagnostics.DiagnosticSource => 27
	i64 u0x581a8bd5cfda563e, ; 273: System.Threading.Timer => 151
	i64 u0x588c167a79db6bfb, ; 274: lib_Xamarin.Google.ErrorProne.Annotations.dll.so => 245
	i64 u0x5906028ae5151104, ; 275: Xamarin.AndroidX.Activity.Ktx => 184
	i64 u0x595a356d23e8da9a, ; 276: lib_Microsoft.CSharp.dll.so => 1
	i64 u0x59f9e60b9475085f, ; 277: lib_Xamarin.AndroidX.Annotation.Experimental.dll.so => 186
	i64 u0x5a745f5101a75527, ; 278: lib_System.IO.Compression.FileSystem.dll.so => 44
	i64 u0x5a89a886ae30258d, ; 279: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 198
	i64 u0x5a8f6699f4a1caa9, ; 280: lib_System.Threading.dll.so => 152
	i64 u0x5ae9cd33b15841bf, ; 281: System.ComponentModel => 18
	i64 u0x5b54391bdc6fcfe6, ; 282: System.Private.DataContractSerialization => 88
	i64 u0x5b8109e8e14c5e3e, ; 283: System.Globalization.Extensions.dll => 41
	i64 u0x5bdf16b09da116ab, ; 284: Xamarin.AndroidX.Collection => 193
	i64 u0x5c019d5266093159, ; 285: lib_Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll.so => 220
	i64 u0x5c30a4a35f9cc8c4, ; 286: lib_System.Reflection.Extensions.dll.so => 96
	i64 u0x5c53c29f5073b0c9, ; 287: System.Diagnostics.FileVersionInfo => 28
	i64 u0x5c87463c575c7616, ; 288: lib_System.Globalization.Extensions.dll.so => 41
	i64 u0x5d0a4a29b02d9d3c, ; 289: System.Net.WebHeaderCollection.dll => 80
	i64 u0x5d40c9b15181641f, ; 290: lib_Xamarin.AndroidX.Emoji2.dll.so => 207
	i64 u0x5d6ca10d35e9485b, ; 291: lib_Xamarin.AndroidX.Concurrent.Futures.dll.so => 195
	i64 u0x5d7ec76c1c703055, ; 292: System.Threading.Tasks.Parallel => 147
	i64 u0x5db0cbbd1028510e, ; 293: lib_System.Runtime.InteropServices.dll.so => 110
	i64 u0x5db30905d3e5013b, ; 294: Xamarin.AndroidX.Collection.Jvm.dll => 194
	i64 u0x5e467bc8f09ad026, ; 295: System.Collections.Specialized.dll => 11
	i64 u0x5e5173b3208d97e7, ; 296: System.Runtime.Handles.dll => 107
	i64 u0x5ea92fdb19ec8c4c, ; 297: System.Text.Encodings.Web.dll => 139
	i64 u0x5eb8046dd40e9ac3, ; 298: System.ComponentModel.Primitives => 16
	i64 u0x5ec272d219c9aba4, ; 299: System.Security.Cryptography.Csp.dll => 124
	i64 u0x5eee1376d94c7f5e, ; 300: System.Net.HttpListener.dll => 67
	i64 u0x5f36ccf5c6a57e24, ; 301: System.Xml.ReaderWriter.dll => 160
	i64 u0x5f4294b9b63cb842, ; 302: System.Data.Common => 22
	i64 u0x5f7399e166075632, ; 303: lib_SQLitePCLRaw.lib.e_sqlite3.android.dll.so => 181
	i64 u0x5fa6da9c3cd8142a, ; 304: lib_Xamarin.KotlinX.Serialization.Core.dll.so => 255
	i64 u0x5fac98e0b37a5b9d, ; 305: System.Runtime.CompilerServices.Unsafe.dll => 104
	i64 u0x60cd4e33d7e60134, ; 306: Xamarin.KotlinX.Coroutines.Core.Jvm => 254
	i64 u0x60f62d786afcf130, ; 307: System.Memory => 64
	i64 u0x61bb78c89f867353, ; 308: System.IO => 58
	i64 u0x61d88f399afb2f45, ; 309: lib_System.Runtime.Loader.dll.so => 112
	i64 u0x622eef6f9e59068d, ; 310: System.Private.CoreLib => 177
	i64 u0x63cdbd66ac39bb46, ; 311: lib_Microsoft.VisualStudio.DesignTools.XamlTapContract.dll.so => 260
	i64 u0x63d5e3aa4ef9b931, ; 312: Xamarin.KotlinX.Coroutines.Android.dll => 252
	i64 u0x63f1f6883c1e23c2, ; 313: lib_System.Collections.Immutable.dll.so => 9
	i64 u0x6400f68068c1e9f1, ; 314: Xamarin.Google.Android.Material.dll => 244
	i64 u0x640e3b14dbd325c2, ; 315: System.Security.Cryptography.Algorithms.dll => 122
	i64 u0x64587004560099b9, ; 316: System.Reflection => 100
	i64 u0x64b1529a438a3c45, ; 317: lib_System.Runtime.Handles.dll.so => 107
	i64 u0x64b61dd9da8a4d57, ; 318: System.Net.ServerSentEvents.dll => 76
	i64 u0x6565fba2cd8f235b, ; 319: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 223
	i64 u0x65ece51227bfa724, ; 320: lib_System.Runtime.Numerics.dll.so => 113
	i64 u0x661722438787b57f, ; 321: Xamarin.AndroidX.Annotation.Jvm.dll => 187
	i64 u0x6679b2337ee6b22a, ; 322: lib_System.IO.FileSystem.Primitives.dll.so => 49
	i64 u0x667c66a03dd97d40, ; 323: System.Linq.AsyncEnumerable.dll => 59
	i64 u0x6692e924eade1b29, ; 324: lib_System.Console.dll.so => 20
	i64 u0x66a4e5c6a3fb0bae, ; 325: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 222
	i64 u0x66d13304ce1a3efa, ; 326: Xamarin.AndroidX.CursorAdapter => 202
	i64 u0x674303f65d8fad6f, ; 327: lib_System.Net.Quic.dll.so => 73
	i64 u0x6756ca4cad62e9d6, ; 328: lib_Xamarin.AndroidX.ConstraintLayout.Core.dll.so => 197
	i64 u0x67c0802770244408, ; 329: System.Windows.dll => 158
	i64 u0x68100b69286e27cd, ; 330: lib_System.Formats.Tar.dll.so => 39
	i64 u0x6872ec7a2e36b1ac, ; 331: System.Drawing.Primitives.dll => 35
	i64 u0x68bb2c417aa9b61c, ; 332: Xamarin.KotlinX.AtomicFU.dll => 250
	i64 u0x68fbbbe2eb455198, ; 333: System.Formats.Asn1 => 38
	i64 u0x699dffb2427a2d71, ; 334: SQLitePCLRaw.lib.e_sqlite3.android.dll => 181
	i64 u0x6a4d7577b2317255, ; 335: System.Runtime.InteropServices.dll => 110
	i64 u0x6afcedb171067e2b, ; 336: System.Core.dll => 21
	i64 u0x6bef98e124147c24, ; 337: Xamarin.Jetbrains.Annotations => 247
	i64 u0x6ce874bff138ce2b, ; 338: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 221
	i64 u0x6d70755158ca866e, ; 339: lib_System.ComponentModel.EventBasedAsync.dll.so => 15
	i64 u0x6d7eeca99577fc8b, ; 340: lib_System.Net.WebProxy.dll.so => 81
	i64 u0x6d8515b19946b6a2, ; 341: System.Net.WebProxy.dll => 81
	i64 u0x6d86d56b84c8eb71, ; 342: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 202
	i64 u0x6e79c6bd8627412a, ; 343: Xamarin.AndroidX.SavedState.SavedState.Ktx => 234
	i64 u0x6e838d9a2a6f6c9e, ; 344: lib_System.ValueTuple.dll.so => 155
	i64 u0x6e9965ce1095e60a, ; 345: lib_System.Core.dll.so => 21
	i64 u0x6ffc4967cc47ba57, ; 346: System.IO.FileSystem.Watcher.dll => 50
	i64 u0x701cd46a1c25a5fe, ; 347: System.IO.FileSystem.dll => 51
	i64 u0x71485e7ffdb4b958, ; 348: System.Reflection.Extensions => 96
	i64 u0x71ad672adbe48f35, ; 349: System.ComponentModel.Primitives.dll => 16
	i64 u0x720f102581a4a5c8, ; 350: Xamarin.AndroidX.Core.ViewTree => 201
	i64 u0x725f5a9e82a45c81, ; 351: System.Security.Cryptography.Encoding => 125
	i64 u0x72e0300099accce1, ; 352: System.Xml.XPath.XDocument => 163
	i64 u0x730bfb248998f67a, ; 353: System.IO.Compression.ZipFile => 45
	i64 u0x732b2d67b9e5c47b, ; 354: Xamarin.Google.ErrorProne.Annotations.dll => 245
	i64 u0x73a6be34e822f9d1, ; 355: lib_System.Runtime.Serialization.dll.so => 118
	i64 u0x73e4ce94e2eb6ffc, ; 356: lib_System.Memory.dll.so => 64
	i64 u0x74327ad47b9741a4, ; 357: Xamarin.AndroidX.NavigationEvent.Android.dll => 228
	i64 u0x743a1eccf080489a, ; 358: WindowsBase.dll => 169
	i64 u0x75c326eb821b85c4, ; 359: lib_System.ComponentModel.DataAnnotations.dll.so => 14
	i64 u0x76012e7334db86e5, ; 360: lib_Xamarin.AndroidX.SavedState.dll.so => 232
	i64 u0x76ca07b878f44da0, ; 361: System.Runtime.Numerics.dll => 113
	i64 u0x7736c8a96e51a061, ; 362: lib_Xamarin.AndroidX.Annotation.Jvm.dll.so => 187
	i64 u0x778a805e625329ef, ; 363: System.Linq.Parallel => 61
	i64 u0x779290cc2b801eb7, ; 364: Xamarin.KotlinX.AtomicFU.Jvm => 251
	i64 u0x77d9074d8f33a303, ; 365: lib_System.Net.ServerSentEvents.dll.so => 76
	i64 u0x77f8a4acc2fdc449, ; 366: System.Security.Cryptography.Cng.dll => 123
	i64 u0x782c5d8eb99ff201, ; 367: lib_Microsoft.VisualBasic.Core.dll.so => 2
	i64 u0x78a45e51311409b6, ; 368: Xamarin.AndroidX.Fragment.dll => 209
	i64 u0x78ed4ab8f9d800a1, ; 369: Xamarin.AndroidX.Lifecycle.ViewModel => 221
	i64 u0x7a39601d6f0bb831, ; 370: lib_Xamarin.KotlinX.AtomicFU.dll.so => 250
	i64 u0x7a5207a7c82d30b4, ; 371: lib_Xamarin.JSpecify.dll.so => 248
	i64 u0x7a7e7eddf79c5d26, ; 372: lib_Xamarin.AndroidX.Lifecycle.ViewModel.dll.so => 221
	i64 u0x7a9a57d43b0845fa, ; 373: System.AppContext => 6
	i64 u0x7ad0f4f1e5d08183, ; 374: Xamarin.AndroidX.Collection.dll => 193
	i64 u0x7b13d9eaa944ade8, ; 375: Xamarin.AndroidX.DynamicAnimation.dll => 206
	i64 u0x7bef86a4335c4870, ; 376: System.ComponentModel.TypeConverter => 17
	i64 u0x7c41d387501568ba, ; 377: System.Net.WebClient.dll => 79
	i64 u0x7c482cd79bd24b13, ; 378: lib_Xamarin.AndroidX.ConstraintLayout.dll.so => 196
	i64 u0x7cd2ec8eaf5241cd, ; 379: System.Security.dll => 133
	i64 u0x7cf9ae50dd350622, ; 380: Xamarin.Jetbrains.Annotations.dll => 247
	i64 u0x7d8ee2bdc8e3aad1, ; 381: System.Numerics.Vectors => 85
	i64 u0x7dfc3d6d9d8d7b70, ; 382: System.Collections => 12
	i64 u0x7e2e564fa2f76c65, ; 383: lib_System.Diagnostics.Tracing.dll.so => 34
	i64 u0x7e302e110e1e1346, ; 384: lib_System.Security.Claims.dll.so => 121
	i64 u0x7e4465b3f78ad8d0, ; 385: Xamarin.KotlinX.Serialization.Core.dll => 255
	i64 u0x7e571cad5915e6c3, ; 386: lib_Xamarin.AndroidX.Lifecycle.Process.dll.so => 216
	i64 u0x7e6ac99e4e8df72f, ; 387: System.IO.Hashing => 176
	i64 u0x7e6b1ca712437d7d, ; 388: Xamarin.AndroidX.Emoji2.ViewsHelper => 208
	i64 u0x7e946809d6008ef2, ; 389: lib_System.ObjectModel.dll.so => 87
	i64 u0x7ecc13347c8fd849, ; 390: lib_System.ComponentModel.dll.so => 18
	i64 u0x7f00ddd9b9ca5a13, ; 391: Xamarin.AndroidX.ViewPager.dll => 242
	i64 u0x7fbd557c99b3ce6f, ; 392: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 215
	i64 u0x8076a9a44a2ca331, ; 393: System.Net.Quic => 73
	i64 u0x80b7e726b0280681, ; 394: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 258
	i64 u0x80da183a87731838, ; 395: System.Reflection.Metadata => 97
	i64 u0x80fa55b6d1b0be99, ; 396: SQLitePCLRaw.provider.e_sqlite3 => 182
	i64 u0x812c069d5cdecc17, ; 397: System.dll => 168
	i64 u0x81381be520a60adb, ; 398: Xamarin.AndroidX.Interpolator.dll => 212
	i64 u0x81657cec2b31e8aa, ; 399: System.Net => 84
	i64 u0x8277f2be6b5ce05f, ; 400: Xamarin.AndroidX.AppCompat => 188
	i64 u0x828f06563b30bc50, ; 401: lib_Xamarin.AndroidX.CardView.dll.so => 192
	i64 u0x82920a8d9194a019, ; 402: Xamarin.KotlinX.AtomicFU.Jvm.dll => 251
	i64 u0x82b399cb01b531c4, ; 403: lib_System.Web.dll.so => 157
	i64 u0x82df8f5532a10c59, ; 404: lib_System.Drawing.dll.so => 36
	i64 u0x82f0b6e911d13535, ; 405: lib_System.Transactions.dll.so => 154
	i64 u0x83144699b312ad81, ; 406: SQLite-net.dll => 178
	i64 u0x846ce984efea52c7, ; 407: System.Threading.Tasks.Parallel.dll => 147
	i64 u0x84ae73148a4557d2, ; 408: lib_System.IO.Pipes.dll.so => 56
	i64 u0x84b01102c12a9232, ; 409: System.Runtime.Serialization.Json.dll => 115
	i64 u0x850c5ba0b57ce8e7, ; 410: lib_Xamarin.AndroidX.Collection.dll.so => 193
	i64 u0x851d02edd334b044, ; 411: Xamarin.AndroidX.VectorDrawable => 239
	i64 u0x85c919db62150978, ; 412: Xamarin.AndroidX.Transition.dll => 238
	i64 u0x8662aaeb94fef37f, ; 413: lib_System.Dynamic.Runtime.dll.so => 37
	i64 u0x86b62cb077ec4fd7, ; 414: System.Runtime.Serialization.Xml => 117
	i64 u0x86f1940283a92e3d, ; 415: lib_Agrosens.dll.so => 0
	i64 u0x8706ffb12bf3f53d, ; 416: Xamarin.AndroidX.Annotation.Experimental => 186
	i64 u0x872a5b14c18d328c, ; 417: System.ComponentModel.DataAnnotations => 14
	i64 u0x87c69b87d9283884, ; 418: lib_System.Threading.Thread.dll.so => 149
	i64 u0x87f6569b25707834, ; 419: System.IO.Compression.Brotli.dll => 43
	i64 u0x88926583efe7ee86, ; 420: Xamarin.AndroidX.Activity.Ktx.dll => 184
	i64 u0x88ba6bc4f7762b03, ; 421: lib_System.Reflection.dll.so => 100
	i64 u0x88bda98e0cffb7a9, ; 422: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 254
	i64 u0x8930322c7bd8f768, ; 423: netstandard => 171
	i64 u0x897a606c9e39c75f, ; 424: lib_System.ComponentModel.Primitives.dll.so => 16
	i64 u0x898a5c6bc9e47ec1, ; 425: lib_Xamarin.AndroidX.SavedState.SavedState.Android.dll.so => 233
	i64 u0x89911a22005b92b7, ; 426: System.IO.FileSystem.DriveInfo.dll => 48
	i64 u0x89c5188089ec2cd5, ; 427: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 109
	i64 u0x8a19e3dc71b34b2c, ; 428: System.Reflection.TypeExtensions.dll => 99
	i64 u0x8ad229ea26432ee2, ; 429: Xamarin.AndroidX.Loader => 226
	i64 u0x8b4ff5d0fdd5faa1, ; 430: lib_System.Diagnostics.DiagnosticSource.dll.so => 27
	i64 u0x8b541d476eb3774c, ; 431: System.Security.Principal.Windows => 130
	i64 u0x8b8d01333a96d0b5, ; 432: System.Diagnostics.Process.dll => 29
	i64 u0x8cb8f612b633affb, ; 433: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 234
	i64 u0x8cdfdb4ce85fb925, ; 434: lib_System.Security.Principal.Windows.dll.so => 130
	i64 u0x8cdfe7b8f4caa426, ; 435: System.IO.Compression.FileSystem => 44
	i64 u0x8d0f420977c2c1c7, ; 436: Xamarin.AndroidX.CursorAdapter.dll => 202
	i64 u0x8d52f7ea2796c531, ; 437: Xamarin.AndroidX.Emoji2.dll => 207
	i64 u0x8d7b8ab4b3310ead, ; 438: System.Threading => 152
	i64 u0x8da188285aadfe8e, ; 439: System.Collections.Concurrent => 8
	i64 u0x8e8f269ad1e1ff94, ; 440: lib_Xamarin.AndroidX.Tracing.Tracing.Android.dll.so => 237
	i64 u0x8ef9414937d93a0a, ; 441: SQLitePCLRaw.core.dll => 180
	i64 u0x8f44b45eb046bbd1, ; 442: System.ServiceModel.Web.dll => 134
	i64 u0x8fbf5b0114c6dcef, ; 443: System.Globalization.dll => 42
	i64 u0x8fcc8c2a81f3d9e7, ; 444: Xamarin.KotlinX.Serialization.Core => 255
	i64 u0x8fd27d934d7b3a55, ; 445: SQLitePCLRaw.core => 180
	i64 u0x90263f8448b8f572, ; 446: lib_System.Diagnostics.TraceSource.dll.so => 33
	i64 u0x903101b46fb73a04, ; 447: _Microsoft.Android.Resource.Designer => 261
	i64 u0x90393bd4865292f3, ; 448: lib_System.IO.Compression.dll.so => 46
	i64 u0x905e2b8e7ae91ae6, ; 449: System.Threading.Tasks.Extensions.dll => 146
	i64 u0x90634f86c5ebe2b5, ; 450: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 222
	i64 u0x91418dc638b29e68, ; 451: lib_Xamarin.AndroidX.CustomView.dll.so => 203
	i64 u0x9157bd523cd7ed36, ; 452: lib_System.Text.Json.dll.so => 140
	i64 u0x91a74f07b30d37e2, ; 453: System.Linq.dll => 63
	i64 u0x91cb86ea3b17111d, ; 454: System.ServiceModel.Web => 134
	i64 u0x92054e486c0c7ea7, ; 455: System.IO.FileSystem.DriveInfo => 48
	i64 u0x928614058c40c4cd, ; 456: lib_System.Xml.XPath.XDocument.dll.so => 163
	i64 u0x92b138fffca2b01e, ; 457: lib_Xamarin.AndroidX.Arch.Core.Runtime.dll.so => 191
	i64 u0x9388aad9b7ae40ce, ; 458: lib_Xamarin.AndroidX.Lifecycle.Common.dll.so => 213
	i64 u0x941c00d21e5c0679, ; 459: lib_Xamarin.AndroidX.Transition.dll.so => 238
	i64 u0x944077d8ca3c6580, ; 460: System.IO.Compression.dll => 46
	i64 u0x948cffedc8ed7960, ; 461: System.Xml => 167
	i64 u0x94bbeab0d4764588, ; 462: System.IO.Hashing.dll => 176
	i64 u0x94c8990839c4bdb1, ; 463: lib_Xamarin.AndroidX.Interpolator.dll.so => 212
	i64 u0x97b8c771ea3e4220, ; 464: System.ComponentModel.dll => 18
	i64 u0x97e144c9d3c6976e, ; 465: System.Collections.Concurrent.dll => 8
	i64 u0x9843944103683dd3, ; 466: Xamarin.AndroidX.Core.Core.Ktx => 200
	i64 u0x98b05cc81e6f333c, ; 467: Xamarin.AndroidX.SavedState.SavedState.Android.dll => 233
	i64 u0x98d720cc4597562c, ; 468: System.Security.Cryptography.OpenSsl => 126
	i64 u0x991d510397f92d9d, ; 469: System.Linq.Expressions => 60
	i64 u0x996ceeb8a3da3d67, ; 470: System.Threading.Overlapped.dll => 144
	i64 u0x9a01b1da98b6ee10, ; 471: Xamarin.AndroidX.Lifecycle.Runtime.dll => 217
	i64 u0x9b211a749105beac, ; 472: System.Transactions.Local => 153
	i64 u0x9b8734714671022d, ; 473: System.Threading.Tasks.Dataflow.dll => 145
	i64 u0x9bc6aea27fbf034f, ; 474: lib_Xamarin.KotlinX.Coroutines.Core.dll.so => 253
	i64 u0x9bd8cc74558ad4c7, ; 475: Xamarin.KotlinX.AtomicFU => 250
	i64 u0x9c244ac7cda32d26, ; 476: System.Security.Cryptography.X509Certificates.dll => 128
	i64 u0x9c465f280cf43733, ; 477: lib_Xamarin.KotlinX.Coroutines.Android.dll.so => 252
	i64 u0x9c8f6872beab6408, ; 478: System.Xml.XPath.XDocument.dll => 163
	i64 u0x9ce01cf91101ae23, ; 479: System.Xml.XmlDocument => 165
	i64 u0x9d128180c81d7ce6, ; 480: Xamarin.AndroidX.CustomView.PoolingContainer => 204
	i64 u0x9d5dbcf5a48583fe, ; 481: lib_Xamarin.AndroidX.Activity.dll.so => 183
	i64 u0x9e4b95dec42769f7, ; 482: System.Diagnostics.Debug.dll => 26
	i64 u0x9ef542cf1f78c506, ; 483: Xamarin.AndroidX.Lifecycle.LiveData.Core => 215
	i64 u0xa00832eb975f56a8, ; 484: lib_System.Net.dll.so => 84
	i64 u0xa0d8259f4cc284ec, ; 485: lib_System.Security.Cryptography.dll.so => 129
	i64 u0xa0ff9b3e34d92f11, ; 486: lib_System.Resources.Writer.dll.so => 103
	i64 u0xa12fbfb4da97d9f3, ; 487: System.Threading.Timer.dll => 151
	i64 u0xa1440773ee9d341e, ; 488: Xamarin.Google.Android.Material => 244
	i64 u0xa2572680829d2c7c, ; 489: System.IO.Pipelines.dll => 54
	i64 u0xa26597e57ee9c7f6, ; 490: System.Xml.XmlDocument.dll => 165
	i64 u0xa308401900e5bed3, ; 491: lib_mscorlib.dll.so => 170
	i64 u0xa395572e7da6c99d, ; 492: lib_System.Security.dll.so => 133
	i64 u0xa3e683f24b43af6f, ; 493: System.Dynamic.Runtime.dll => 37
	i64 u0xa4145becdee3dc4f, ; 494: Xamarin.AndroidX.VectorDrawable.Animated => 240
	i64 u0xa4edc8f2ceae241a, ; 495: System.Data.Common.dll => 22
	i64 u0xa5494f40f128ce6a, ; 496: System.Runtime.Serialization.Formatters.dll => 114
	i64 u0xa54b74df83dce92b, ; 497: System.Reflection.DispatchProxy => 92
	i64 u0xa5b7152421ed6d98, ; 498: lib_System.IO.FileSystem.Watcher.dll.so => 50
	i64 u0xa5c3844f17b822db, ; 499: lib_System.Linq.Parallel.dll.so => 61
	i64 u0xa5ce5c755bde8cb8, ; 500: lib_System.Security.Cryptography.Csp.dll.so => 124
	i64 u0xa5e599d1e0524750, ; 501: System.Numerics.Vectors.dll => 85
	i64 u0xa5f1ba49b85dd355, ; 502: System.Security.Cryptography.dll => 129
	i64 u0xa61975a5a37873ea, ; 503: lib_System.Xml.XmlSerializer.dll.so => 166
	i64 u0xa66cbee0130865f7, ; 504: lib_WindowsBase.dll.so => 169
	i64 u0xa67dbee13e1df9ca, ; 505: Xamarin.AndroidX.SavedState.dll => 232
	i64 u0xa68a420042bb9b1f, ; 506: Xamarin.AndroidX.DrawerLayout.dll => 205
	i64 u0xa75386b5cb9595aa, ; 507: Xamarin.AndroidX.Lifecycle.Runtime.Android => 218
	i64 u0xa763fbb98df8d9fb, ; 508: lib_Microsoft.Win32.Primitives.dll.so => 4
	i64 u0xa78ce3745383236a, ; 509: Xamarin.AndroidX.Lifecycle.Common.Jvm => 214
	i64 u0xa7eab29ed44b4e7a, ; 510: Mono.Android.Export => 173
	i64 u0xa8195217cbf017b7, ; 511: Microsoft.VisualBasic.Core => 2
	i64 u0xa859a95830f367ff, ; 512: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll.so => 223
	i64 u0xa8b52f21e0dbe690, ; 513: System.Runtime.Serialization.dll => 118
	i64 u0xa8c84ce526c2b4bd, ; 514: Microsoft.VisualStudio.DesignTools.XamlTapContract.dll => 260
	i64 u0xa8ee4ed7de2efaee, ; 515: Xamarin.AndroidX.Annotation.dll => 185
	i64 u0xa95590e7c57438a4, ; 516: System.Configuration => 19
	i64 u0xaa443ac34067eeef, ; 517: System.Private.Xml.dll => 91
	i64 u0xaa52de307ef5d1dd, ; 518: System.Net.Http => 66
	i64 u0xaa9a7b0214a5cc5c, ; 519: System.Diagnostics.StackTrace.dll => 30
	i64 u0xaaeebf9c8d9c28aa, ; 520: Xamarin.AndroidX.Graphics.Shapes => 210
	i64 u0xab9af77b5b67a0b8, ; 521: Xamarin.AndroidX.ConstraintLayout.Core => 197
	i64 u0xab9c1b2687d86b0b, ; 522: lib_System.Linq.Expressions.dll.so => 60
	i64 u0xac2af3fa195a15ce, ; 523: System.Runtime.Numerics => 113
	i64 u0xac5376a2a538dc10, ; 524: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 215
	i64 u0xac5acae88f60357e, ; 525: System.Diagnostics.Tools.dll => 32
	i64 u0xac79c7e46047ad98, ; 526: System.Security.Principal.Windows.dll => 130
	i64 u0xac98d31068e24591, ; 527: System.Xml.XDocument => 162
	i64 u0xacdd9e4180d56dda, ; 528: Xamarin.AndroidX.Concurrent.Futures => 195
	i64 u0xacf42eea7ef9cd12, ; 529: System.Threading.Channels => 143
	i64 u0xadbb53caf78a79d2, ; 530: System.Web.HttpUtility => 156
	i64 u0xadc90ab061a9e6e4, ; 531: System.ComponentModel.TypeConverter.dll => 17
	i64 u0xadf4cf30debbeb9a, ; 532: System.Net.ServicePoint.dll => 77
	i64 u0xadf511667bef3595, ; 533: System.Net.Security => 75
	i64 u0xae0aaa94fdcfce0f, ; 534: System.ComponentModel.EventBasedAsync.dll => 15
	i64 u0xae282bcd03739de7, ; 535: Java.Interop => 172
	i64 u0xae53579c90db1107, ; 536: System.ObjectModel.dll => 87
	i64 u0xae7ea18c61eef394, ; 537: SQLite-net => 178
	i64 u0xaec7c0c7e2ed4575, ; 538: lib_Xamarin.KotlinX.AtomicFU.Jvm.dll.so => 251
	i64 u0xaf732d0b2193b8f5, ; 539: System.Security.Cryptography.OpenSsl.dll => 126
	i64 u0xafe29f45095518e7, ; 540: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 224
	i64 u0xb03ae931fb25607e, ; 541: Xamarin.AndroidX.ConstraintLayout => 196
	i64 u0xb0ac21bec8f428c5, ; 542: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.Android.dll => 220
	i64 u0xb0bb43dc52ea59f9, ; 543: System.Diagnostics.Tracing.dll => 34
	i64 u0xb1dd05401aa8ee63, ; 544: System.Security.AccessControl => 120
	i64 u0xb220631954820169, ; 545: System.Text.RegularExpressions => 141
	i64 u0xb2376e1dbf8b4ed7, ; 546: System.Security.Cryptography.Csp => 124
	i64 u0xb2a1959fe95c5402, ; 547: lib_System.Runtime.InteropServices.JavaScript.dll.so => 108
	i64 u0xb3011a0a57f7ffb2, ; 548: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 258
	i64 u0xb34f09b530d826d6, ; 549: Xamarin.AndroidX.NavigationEvent.dll => 227
	i64 u0xb3874072ee0ecf8c, ; 550: Xamarin.AndroidX.VectorDrawable.Animated.dll => 240
	i64 u0xb3f0a0fcda8d3ebc, ; 551: Xamarin.AndroidX.CardView => 192
	i64 u0xb4bd7015ecee9d86, ; 552: System.IO.Pipelines => 54
	i64 u0xb4c53d9749c5f226, ; 553: lib_System.IO.FileSystem.AccessControl.dll.so => 47
	i64 u0xb4ff710863453fda, ; 554: System.Diagnostics.FileVersionInfo.dll => 28
	i64 u0xb54092076b15e062, ; 555: System.Threading.AccessControl => 142
	i64 u0xb5c38bf497a4cfe2, ; 556: lib_System.Threading.Tasks.dll.so => 148
	i64 u0xb5ea31d5244c6626, ; 557: System.Threading.ThreadPool.dll => 150
	i64 u0xb7212c4683a94afe, ; 558: System.Drawing.Primitives => 35
	i64 u0xb81a2c6e0aee50fe, ; 559: lib_System.Private.CoreLib.dll.so => 177
	i64 u0xb8c60af47c08d4da, ; 560: System.Net.ServicePoint => 77
	i64 u0xb8e68d20aad91196, ; 561: lib_System.Xml.XPath.dll.so => 164
	i64 u0xb9185c33a1643eed, ; 562: Microsoft.CSharp.dll => 1
	i64 u0xb960d6b2200ba320, ; 563: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.Android.dll => 225
	i64 u0xba4670aa94a2b3c6, ; 564: lib_System.Xml.XDocument.dll.so => 162
	i64 u0xba48785529705af9, ; 565: System.Collections.dll => 12
	i64 u0xba965b8c86359996, ; 566: lib_System.Windows.dll.so => 158
	i64 u0xbb286883bc35db36, ; 567: System.Transactions.dll => 154
	i64 u0xbb65706fde942ce3, ; 568: System.Net.Sockets => 78
	i64 u0xbba28979413cad9e, ; 569: lib_System.Runtime.CompilerServices.VisualC.dll.so => 105
	i64 u0xbbd180354b67271a, ; 570: System.Runtime.Serialization.Formatters => 114
	i64 u0xbc22a245dab70cb4, ; 571: lib_SQLitePCLRaw.provider.e_sqlite3.dll.so => 182
	i64 u0xbc260cdba33291a3, ; 572: Xamarin.AndroidX.Arch.Core.Common.dll => 190
	i64 u0xbd0e2c0d55246576, ; 573: System.Net.Http.dll => 66
	i64 u0xbd3fbd85b9e1cb29, ; 574: lib_System.Net.HttpListener.dll.so => 67
	i64 u0xbd437a2cdb333d0d, ; 575: Xamarin.AndroidX.ViewPager2 => 243
	i64 u0xbd4f572d2bd0a789, ; 576: System.IO.Compression.ZipFile.dll => 45
	i64 u0xbd877b14d0b56392, ; 577: System.Runtime.Intrinsics.dll => 111
	i64 u0xbe65a49036345cf4, ; 578: lib_System.Buffers.dll.so => 7
	i64 u0xbee38d4a88835966, ; 579: Xamarin.AndroidX.AppCompat.AppCompatResources => 189
	i64 u0xbef9919db45b4ca7, ; 580: System.IO.Pipes.AccessControl => 55
	i64 u0xbf0fa68611139208, ; 581: lib_Xamarin.AndroidX.Annotation.dll.so => 185
	i64 u0xbfc1e1fb3095f2b3, ; 582: lib_System.Net.Http.Json.dll.so => 65
	i64 u0xc07cadab29efeba0, ; 583: Xamarin.AndroidX.Core.Core.Ktx.dll => 200
	i64 u0xc0d928351ab5ca77, ; 584: System.Console.dll => 20
	i64 u0xc0f5a221a9383aea, ; 585: System.Runtime.Intrinsics => 111
	i64 u0xc111030af54d7191, ; 586: System.Resources.Writer => 103
	i64 u0xc12b8b3afa48329c, ; 587: lib_System.Linq.dll.so => 63
	i64 u0xc183ca0b74453aa9, ; 588: lib_System.Threading.Tasks.Dataflow.dll.so => 145
	i64 u0xc1ff9ae3cdb6e1e6, ; 589: Xamarin.AndroidX.Activity.dll => 183
	i64 u0xc26c064effb1dea9, ; 590: System.Buffers.dll => 7
	i64 u0xc2902f6cf5452577, ; 591: lib_Mono.Android.Export.dll.so => 173
	i64 u0xc2a3bca55b573141, ; 592: System.IO.FileSystem.Watcher => 50
	i64 u0xc2bcfec99f69365e, ; 593: Xamarin.AndroidX.ViewPager2.dll => 243
	i64 u0xc30b52815b58ac2c, ; 594: lib_System.Runtime.Serialization.Xml.dll.so => 117
	i64 u0xc36d7d89c652f455, ; 595: System.Threading.Overlapped => 144
	i64 u0xc3c86c1e5e12f03d, ; 596: WindowsBase => 169
	i64 u0xc421b61fd853169d, ; 597: lib_System.Net.WebSockets.Client.dll.so => 82
	i64 u0xc463e077917aa21d, ; 598: System.Runtime.Serialization.Json => 115
	i64 u0xc4d3858ed4d08512, ; 599: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 224
	i64 u0xc50fded0ded1418c, ; 600: lib_System.ComponentModel.TypeConverter.dll.so => 17
	i64 u0xc519125d6bc8fb11, ; 601: lib_System.Net.Requests.dll.so => 74
	i64 u0xc5325b2fcb37446f, ; 602: lib_System.Private.Xml.dll.so => 91
	i64 u0xc5a0f4b95a699af7, ; 603: lib_System.Private.Uri.dll.so => 89
	i64 u0xc5cdcd5b6277579e, ; 604: lib_System.Security.Cryptography.Algorithms.dll.so => 122
	i64 u0xc5ec286825cb0bf4, ; 605: Xamarin.AndroidX.Tracing.Tracing => 236
	i64 u0xc6706bc8aa7fe265, ; 606: Xamarin.AndroidX.Annotation.Jvm => 187
	i64 u0xc7c01e7d7c93a110, ; 607: System.Text.Encoding.Extensions.dll => 137
	i64 u0xc7ce851898a4548e, ; 608: lib_System.Web.HttpUtility.dll.so => 156
	i64 u0xc809d4089d2556b2, ; 609: System.Runtime.InteropServices.JavaScript.dll => 108
	i64 u0xc858a28d9ee5a6c5, ; 610: lib_System.Collections.Specialized.dll.so => 11
	i64 u0xc8ac7c6bf1c2ec51, ; 611: System.Reflection.DispatchProxy.dll => 92
	i64 u0xc9c62c8f354ac568, ; 612: lib_System.Diagnostics.TextWriterTraceListener.dll.so => 31
	i64 u0xca5801070d9fccfb, ; 613: System.Text.Encoding => 138
	i64 u0xcadbc92899a777f0, ; 614: Xamarin.AndroidX.Startup.StartupRuntime => 235
	i64 u0xcbb5f80c7293e696, ; 615: lib_System.Globalization.Calendars.dll.so => 40
	i64 u0xcbd4fdd9cef4a294, ; 616: lib__Microsoft.Android.Resource.Designer.dll.so => 261
	i64 u0xcc2876b32ef2794c, ; 617: lib_System.Text.RegularExpressions.dll.so => 141
	i64 u0xcc5c3bb714c4561e, ; 618: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 254
	i64 u0xcc76886e09b88260, ; 619: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 256
	i64 u0xcc9fa2923aa1c9ef, ; 620: System.Diagnostics.Contracts.dll => 25
	i64 u0xccae9bb73e2326bd, ; 621: lib_System.IO.Hashing.dll.so => 176
	i64 u0xcd10a42808629144, ; 622: System.Net.Requests => 74
	i64 u0xcdca1b920e9f53ba, ; 623: Xamarin.AndroidX.Interpolator => 212
	i64 u0xcde1fa22dc303670, ; 624: Microsoft.VisualStudio.DesignTools.XamlTapContract => 260
	i64 u0xcf23d8093f3ceadf, ; 625: System.Diagnostics.DiagnosticSource.dll => 27
	i64 u0xcf5ff6b6b2c4c382, ; 626: System.Net.Mail.dll => 68
	i64 u0xcf8fc898f98b0d34, ; 627: System.Private.Xml.Linq => 90
	i64 u0xd04b5f59ed596e31, ; 628: System.Reflection.Metadata.dll => 97
	i64 u0xd063299fcfc0c93f, ; 629: lib_System.Runtime.Serialization.Json.dll.so => 115
	i64 u0xd0de8a113e976700, ; 630: System.Diagnostics.TextWriterTraceListener => 31
	i64 u0xd0fc33d5ae5d4cb8, ; 631: System.Runtime.Extensions => 106
	i64 u0xd1194e1d8a8de83c, ; 632: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 214
	i64 u0xd12beacdfc14f696, ; 633: System.Dynamic.Runtime => 37
	i64 u0xd198e7ce1b6a8344, ; 634: System.Net.Quic.dll => 73
	i64 u0xd3144156a3727ebe, ; 635: Xamarin.Google.Guava.ListenableFuture => 246
	i64 u0xd333d0af9e423810, ; 636: System.Runtime.InteropServices => 110
	i64 u0xd33a415cb4278969, ; 637: System.Security.Cryptography.Encoding.dll => 125
	i64 u0xd3426d966bb704f5, ; 638: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 189
	i64 u0xd3651b6fc3125825, ; 639: System.Private.Uri.dll => 89
	i64 u0xd3801faafafb7698, ; 640: System.Private.DataContractSerialization.dll => 88
	i64 u0xd3edcc1f25459a50, ; 641: System.Reflection.Emit => 95
	i64 u0xd4fa0abb79079ea9, ; 642: System.Security.Principal.dll => 131
	i64 u0xd5507e11a2b2839f, ; 643: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 224
	i64 u0xd5d04bef8478ea19, ; 644: Xamarin.AndroidX.Tracing.Tracing.dll => 236
	i64 u0xd65786d27a4ad960, ; 645: lib_Microsoft.Maui.Controls.HotReload.Forms.dll.so => 257
	i64 u0xd6694f8359737e4e, ; 646: Xamarin.AndroidX.SavedState => 232
	i64 u0xd6949e129339eae5, ; 647: lib_Xamarin.AndroidX.Core.Core.Ktx.dll.so => 200
	i64 u0xd6de019f6af72435, ; 648: Xamarin.AndroidX.ConstraintLayout.Core.dll => 197
	i64 u0xd6f6d366d4896aef, ; 649: lib_Xamarin.AndroidX.NavigationEvent.Android.dll.so => 228
	i64 u0xd72c760af136e863, ; 650: System.Xml.XmlSerializer.dll => 166
	i64 u0xd753f071e44c2a03, ; 651: lib_System.Security.SecureString.dll.so => 132
	i64 u0xd7f0088bc5ad71f2, ; 652: Xamarin.AndroidX.VersionedParcelable => 241
	i64 u0xd8fb25e28ae30a12, ; 653: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 229
	i64 u0xdad05a11827959a3, ; 654: System.Collections.NonGeneric.dll => 10
	i64 u0xdaefdfe71aa53cf9, ; 655: System.IO.FileSystem.Primitives => 49
	i64 u0xdb58816721c02a59, ; 656: lib_System.Reflection.Emit.ILGeneration.dll.so => 93
	i64 u0xdbf2a779fbc3ac31, ; 657: System.Transactions.Local.dll => 153
	i64 u0xdbf9607a441b4505, ; 658: System.Linq => 63
	i64 u0xdbfc90157a0de9b0, ; 659: lib_System.Text.Encoding.dll.so => 138
	i64 u0xdc75032002d1a212, ; 660: lib_System.Transactions.Local.dll.so => 153
	i64 u0xdca8be7403f92d4f, ; 661: lib_System.Linq.Queryable.dll.so => 62
	i64 u0xdd2b722d78ef5f43, ; 662: System.Runtime.dll => 119
	i64 u0xdd67031857c72f96, ; 663: lib_System.Text.Encodings.Web.dll.so => 139
	i64 u0xdd70765ad6162057, ; 664: Xamarin.JSpecify => 248
	i64 u0xdd92e229ad292030, ; 665: System.Numerics.dll => 86
	i64 u0xde110ae80fa7c2e2, ; 666: System.Xml.XDocument.dll => 162
	i64 u0xde4726fcdf63a198, ; 667: Xamarin.AndroidX.Transition => 238
	i64 u0xde572c2b2fb32f93, ; 668: lib_System.Threading.Tasks.Extensions.dll.so => 146
	i64 u0xdf4b773de8fb1540, ; 669: System.Net.dll => 84
	i64 u0xdf9c7682560a9629, ; 670: System.Net.ServerSentEvents => 76
	i64 u0xdfa254ebb4346068, ; 671: System.Net.Ping => 71
	i64 u0xe0142572c095a480, ; 672: Xamarin.AndroidX.AppCompat.dll => 188
	i64 u0xe021eaa401792a05, ; 673: System.Text.Encoding.dll => 138
	i64 u0xe02f89350ec78051, ; 674: Xamarin.AndroidX.CoordinatorLayout.dll => 198
	i64 u0xe10b760bb1462e7a, ; 675: lib_System.Security.Cryptography.Primitives.dll.so => 127
	i64 u0xe1566bbdb759c5af, ; 676: Microsoft.Maui.Controls.HotReload.Forms.dll => 257
	i64 u0xe192a588d4410686, ; 677: lib_System.IO.Pipelines.dll.so => 54
	i64 u0xe1a08bd3fa539e0d, ; 678: System.Runtime.Loader => 112
	i64 u0xe1a77eb8831f7741, ; 679: System.Security.SecureString.dll => 132
	i64 u0xe1b52f9f816c70ef, ; 680: System.Private.Xml.Linq.dll => 90
	i64 u0xe1e199c8ab02e356, ; 681: System.Data.DataSetExtensions.dll => 23
	i64 u0xe1ecfdb7fff86067, ; 682: System.Net.Security.dll => 75
	i64 u0xe2252a80fe853de4, ; 683: lib_System.Security.Principal.dll.so => 131
	i64 u0xe22fa4c9c645db62, ; 684: System.Diagnostics.TextWriterTraceListener.dll => 31
	i64 u0xe2420585aeceb728, ; 685: System.Net.Requests.dll => 74
	i64 u0xe26692647e6bcb62, ; 686: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 219
	i64 u0xe2ad448dee50fbdf, ; 687: System.Xml.Serialization => 161
	i64 u0xe2d920f978f5d85c, ; 688: System.Data.DataSetExtensions => 23
	i64 u0xe2e426c7714fa0bc, ; 689: Microsoft.Win32.Primitives.dll => 4
	i64 u0xe332bacb3eb4a806, ; 690: Mono.Android.Export.dll => 173
	i64 u0xe3a586956771a0ed, ; 691: lib_SQLite-net.dll.so => 178
	i64 u0xe3b7cbae5ad66c75, ; 692: lib_System.Security.Cryptography.Encoding.dll.so => 125
	i64 u0xe4292b48f3224d5b, ; 693: lib_Xamarin.AndroidX.Core.ViewTree.dll.so => 201
	i64 u0xe4dd7925d04f7f6b, ; 694: lib_Xamarin.AndroidX.NavigationEvent.dll.so => 227
	i64 u0xe4f74a0b5bf9703f, ; 695: System.Runtime.Serialization.Primitives => 116
	i64 u0xe5434e8a119ceb69, ; 696: lib_Mono.Android.dll.so => 175
	i64 u0xe55703b9ce5c038a, ; 697: System.Diagnostics.Tools => 32
	i64 u0xe57013c8afc270b5, ; 698: Microsoft.VisualBasic => 3
	i64 u0xe62913cc36bc07ec, ; 699: System.Xml.dll => 167
	i64 u0xe7bea09c4900a191, ; 700: Xamarin.AndroidX.VectorDrawable.dll => 239
	i64 u0xe7e03cc18dcdeb49, ; 701: lib_System.Diagnostics.StackTrace.dll.so => 30
	i64 u0xe7e147ff99a7a380, ; 702: lib_System.Configuration.dll.so => 19
	i64 u0xe7ef84b36819564e, ; 703: Xamarin.AndroidX.NavigationEvent.Android => 228
	i64 u0xe86b0df4ba9e5db8, ; 704: lib_Xamarin.AndroidX.Lifecycle.Runtime.Android.dll.so => 218
	i64 u0xe896622fe0902957, ; 705: System.Reflection.Emit.dll => 95
	i64 u0xe89a2a9ef110899b, ; 706: System.Drawing.dll => 36
	i64 u0xe8c5f8c100b5934b, ; 707: Microsoft.Win32.Registry => 5
	i64 u0xe98163eb702ae5c5, ; 708: Xamarin.AndroidX.Arch.Core.Runtime => 191
	i64 u0xe994f23ba4c143e5, ; 709: Xamarin.KotlinX.Coroutines.Android => 252
	i64 u0xe9b9c8c0458fd92a, ; 710: System.Windows => 158
	i64 u0xe9d166d87a7f2bdb, ; 711: lib_Xamarin.AndroidX.Startup.StartupRuntime.dll.so => 235
	i64 u0xea5a4efc2ad81d1b, ; 712: Xamarin.Google.ErrorProne.Annotations => 245
	i64 u0xeb2313fe9d65b785, ; 713: Xamarin.AndroidX.ConstraintLayout.dll => 196
	i64 u0xed19c616b3fcb7eb, ; 714: Xamarin.AndroidX.VersionedParcelable.dll => 241
	i64 u0xed60c6fa891c051a, ; 715: lib_Microsoft.VisualStudio.DesignTools.TapContract.dll.so => 259
	i64 u0xedc4817167106c23, ; 716: System.Net.Sockets.dll => 78
	i64 u0xedc632067fb20ff3, ; 717: System.Memory.dll => 64
	i64 u0xee81f5b3f1c4f83b, ; 718: System.Threading.ThreadPool => 150
	i64 u0xeeb7ebb80150501b, ; 719: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 194
	i64 u0xeefc635595ef57f0, ; 720: System.Security.Cryptography.Cng => 123
	i64 u0xef03b1b5a04e9709, ; 721: System.Text.Encoding.CodePages.dll => 136
	i64 u0xef5bcbe61622ee5f, ; 722: Xamarin.AndroidX.Tracing.Tracing.Android.dll => 237
	i64 u0xef602c523fe2e87a, ; 723: lib_Xamarin.Google.Guava.ListenableFuture.dll.so => 246
	i64 u0xefd1e0c4e5c9b371, ; 724: System.Resources.ResourceManager.dll => 102
	i64 u0xefe8f8d5ed3c72ea, ; 725: System.Formats.Tar.dll => 39
	i64 u0xefec0b7fdc57ec42, ; 726: Xamarin.AndroidX.Activity => 183
	i64 u0xeff59cbde4363ec3, ; 727: System.Threading.AccessControl.dll => 142
	i64 u0xf09e47b6ae914f6e, ; 728: System.Net.NameResolution => 69
	i64 u0xf0ac2b489fed2e35, ; 729: lib_System.Diagnostics.Debug.dll.so => 26
	i64 u0xf0bb49dadd3a1fe1, ; 730: lib_System.Net.ServicePoint.dll.so => 77
	i64 u0xf0de2537ee19c6ca, ; 731: lib_System.Net.WebHeaderCollection.dll.so => 80
	i64 u0xf1138779fa181c68, ; 732: lib_Xamarin.AndroidX.Lifecycle.Runtime.dll.so => 217
	i64 u0xf161f4f3c3b7e62c, ; 733: System.Data => 24
	i64 u0xf16eb650d5a464bc, ; 734: System.ValueTuple => 155
	i64 u0xf1c4b4005493d871, ; 735: System.Formats.Asn1.dll => 38
	i64 u0xf22514cfad2d598b, ; 736: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.Android.dll.so => 225
	i64 u0xf2feea356ba760af, ; 737: Xamarin.AndroidX.Arch.Core.Runtime.dll => 191
	i64 u0xf300e085f8acd238, ; 738: lib_System.ServiceProcess.dll.so => 135
	i64 u0xf33cd2d62dbe52f9, ; 739: Xamarin.AndroidX.NavigationEvent => 227
	i64 u0xf34e52b26e7e059d, ; 740: System.Runtime.CompilerServices.VisualC.dll => 105
	i64 u0xf37221fda4ef8830, ; 741: lib_Xamarin.Google.Android.Material.dll.so => 244
	i64 u0xf3ad9b8fb3eefd12, ; 742: lib_System.IO.UnmanagedMemoryStream.dll.so => 57
	i64 u0xf3ddfe05336abf29, ; 743: System => 168
	i64 u0xf408654b2a135055, ; 744: System.Reflection.Emit.ILGeneration.dll => 93
	i64 u0xf4103170a1de5bd0, ; 745: System.Linq.Queryable.dll => 62
	i64 u0xf42d20c23173d77c, ; 746: lib_System.ServiceModel.Web.dll.so => 134
	i64 u0xf4c1dd70a5496a17, ; 747: System.IO.Compression => 46
	i64 u0xf4ecf4b9afc64781, ; 748: System.ServiceProcess.dll => 135
	i64 u0xf4eeeaa566e9b970, ; 749: lib_Xamarin.AndroidX.CustomView.PoolingContainer.dll.so => 204
	i64 u0xf518f63ead11fcd1, ; 750: System.Threading.Tasks => 148
	i64 u0xf5fc7602fe27b333, ; 751: System.Net.WebHeaderCollection => 80
	i64 u0xf6077741019d7428, ; 752: Xamarin.AndroidX.CoordinatorLayout => 198
	i64 u0xf6742cbf457c450b, ; 753: Xamarin.AndroidX.Lifecycle.Runtime.Android.dll => 218
	i64 u0xf70c0a7bf8ccf5af, ; 754: System.Web => 157
	i64 u0xf7e2cac4c45067b3, ; 755: lib_System.Numerics.Vectors.dll.so => 85
	i64 u0xf8aac5ea82de1348, ; 756: System.Linq.Queryable => 62
	i64 u0xf8b77539b362d3ba, ; 757: lib_System.Reflection.Primitives.dll.so => 98
	i64 u0xf8e045dc345b2ea3, ; 758: lib_Xamarin.AndroidX.RecyclerView.dll.so => 230
	i64 u0xf915dc29808193a1, ; 759: System.Web.HttpUtility.dll => 156
	i64 u0xf9be54c8bcf8ff3b, ; 760: System.Security.AccessControl.dll => 120
	i64 u0xfa0e82300e67f913, ; 761: lib_System.AppContext.dll.so => 6
	i64 u0xfa2fdb27e8a2c8e8, ; 762: System.ComponentModel.EventBasedAsync => 15
	i64 u0xfa3f278f288b0e84, ; 763: lib_System.Net.Security.dll.so => 75
	i64 u0xfa645d91e9fc4cba, ; 764: System.Threading.Thread => 149
	i64 u0xfad4d2c770e827f9, ; 765: lib_System.IO.IsolatedStorage.dll.so => 52
	i64 u0xfb022853d73b7fa5, ; 766: lib_SQLitePCLRaw.batteries_v2.dll.so => 179
	i64 u0xfb06dd2338e6f7c4, ; 767: System.Net.Ping.dll => 71
	i64 u0xfb087abe5365e3b7, ; 768: lib_System.Data.DataSetExtensions.dll.so => 23
	i64 u0xfb846e949baff5ea, ; 769: System.Xml.Serialization.dll => 161
	i64 u0xfbad3e4ce4b98145, ; 770: System.Security.Cryptography.X509Certificates => 128
	i64 u0xfbf0a31c9fc34bc4, ; 771: lib_System.Net.Http.dll.so => 66
	i64 u0xfc6b7527cc280b3f, ; 772: lib_System.Runtime.Serialization.Formatters.dll.so => 114
	i64 u0xfc82690c2fe2735c, ; 773: Xamarin.AndroidX.Lifecycle.Process.dll => 216
	i64 u0xfc93fc307d279893, ; 774: System.IO.Pipes.AccessControl.dll => 55
	i64 u0xfcd302092ada6328, ; 775: System.IO.MemoryMappedFiles.dll => 53
	i64 u0xfd22f00870e40ae0, ; 776: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 205
	i64 u0xfd49b3c1a76e2748, ; 777: System.Runtime.InteropServices.RuntimeInformation => 109
	i64 u0xfd536c702f64dc47, ; 778: System.Text.Encoding.Extensions => 137
	i64 u0xfd583f7657b6a1cb, ; 779: Xamarin.AndroidX.Fragment => 209
	i64 u0xfd8dd91a2c26bd5d, ; 780: Xamarin.AndroidX.Lifecycle.Runtime => 217
	i64 u0xfda36abccf05cf5c, ; 781: System.Net.WebSockets.Client => 82
	i64 u0xfddbe9695626a7f5, ; 782: Xamarin.AndroidX.Lifecycle.Common => 213
	i64 u0xff270a55858bac8d, ; 783: System.Security.Principal => 131
	i64 u0xff9b54613e0d2cc8, ; 784: System.Net.Http.Json => 65
	i64 u0xffdb7a971be4ec73 ; 785: System.ValueTuple.dll => 155
], align 16

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [786 x i32] [
	i32 42, i32 253, i32 13, i32 211, i32 107, i32 174, i32 48, i32 188,
	i32 7, i32 182, i32 88, i32 206, i32 72, i32 230, i32 12, i32 104,
	i32 159, i32 19, i32 194, i32 164, i32 208, i32 239, i32 170, i32 10,
	i32 240, i32 98, i32 204, i32 205, i32 13, i32 10, i32 129, i32 97,
	i32 143, i32 39, i32 256, i32 242, i32 175, i32 5, i32 68, i32 132,
	i32 234, i32 207, i32 69, i32 67, i32 0, i32 211, i32 57, i32 203,
	i32 52, i32 43, i32 127, i32 68, i32 83, i32 219, i32 259, i32 161,
	i32 94, i32 101, i32 230, i32 144, i32 154, i32 165, i32 172, i32 83,
	i32 259, i32 248, i32 4, i32 5, i32 51, i32 103, i32 56, i32 122,
	i32 100, i32 171, i32 120, i32 253, i32 21, i32 139, i32 99, i32 256,
	i32 79, i32 235, i32 121, i32 8, i32 168, i32 71, i32 220, i32 231,
	i32 174, i32 148, i32 40, i32 47, i32 30, i32 147, i32 166, i32 28,
	i32 86, i32 236, i32 79, i32 43, i32 29, i32 42, i32 105, i32 119,
	i32 186, i32 45, i32 93, i32 56, i32 151, i32 258, i32 149, i32 102,
	i32 49, i32 20, i32 199, i32 116, i32 179, i32 249, i32 96, i32 58,
	i32 83, i32 172, i32 26, i32 72, i32 229, i32 257, i32 70, i32 33,
	i32 14, i32 141, i32 38, i32 195, i32 136, i32 94, i32 90, i32 152,
	i32 24, i32 140, i32 57, i32 142, i32 51, i32 29, i32 160, i32 34,
	i32 167, i32 209, i32 52, i32 261, i32 92, i32 192, i32 210, i32 35,
	i32 160, i32 9, i32 78, i32 59, i32 55, i32 13, i32 243, i32 190,
	i32 111, i32 223, i32 32, i32 106, i32 86, i32 94, i32 53, i32 98,
	i32 247, i32 58, i32 0, i32 9, i32 104, i32 203, i32 69, i32 242,
	i32 127, i32 231, i32 118, i32 137, i32 233, i32 128, i32 108, i32 249,
	i32 133, i32 246, i32 150, i32 159, i32 199, i32 179, i32 206, i32 231,
	i32 99, i32 24, i32 146, i32 3, i32 170, i32 189, i32 102, i32 164,
	i32 101, i32 201, i32 25, i32 211, i32 95, i32 171, i32 175, i32 184,
	i32 3, i32 237, i32 208, i32 1, i32 116, i32 249, i32 216, i32 33,
	i32 6, i32 210, i32 159, i32 53, i32 87, i32 241, i32 44, i32 106,
	i32 47, i32 140, i32 225, i32 65, i32 226, i32 70, i32 82, i32 60,
	i32 91, i32 157, i32 190, i32 135, i32 112, i32 59, i32 226, i32 229,
	i32 174, i32 136, i32 143, i32 40, i32 181, i32 61, i32 222, i32 81,
	i32 25, i32 36, i32 101, i32 219, i32 72, i32 22, i32 199, i32 123,
	i32 70, i32 109, i32 121, i32 119, i32 213, i32 214, i32 11, i32 2,
	i32 126, i32 117, i32 145, i32 41, i32 89, i32 185, i32 180, i32 177,
	i32 27, i32 151, i32 245, i32 184, i32 1, i32 186, i32 44, i32 198,
	i32 152, i32 18, i32 88, i32 41, i32 193, i32 220, i32 96, i32 28,
	i32 41, i32 80, i32 207, i32 195, i32 147, i32 110, i32 194, i32 11,
	i32 107, i32 139, i32 16, i32 124, i32 67, i32 160, i32 22, i32 181,
	i32 255, i32 104, i32 254, i32 64, i32 58, i32 112, i32 177, i32 260,
	i32 252, i32 9, i32 244, i32 122, i32 100, i32 107, i32 76, i32 223,
	i32 113, i32 187, i32 49, i32 59, i32 20, i32 222, i32 202, i32 73,
	i32 197, i32 158, i32 39, i32 35, i32 250, i32 38, i32 181, i32 110,
	i32 21, i32 247, i32 221, i32 15, i32 81, i32 81, i32 202, i32 234,
	i32 155, i32 21, i32 50, i32 51, i32 96, i32 16, i32 201, i32 125,
	i32 163, i32 45, i32 245, i32 118, i32 64, i32 228, i32 169, i32 14,
	i32 232, i32 113, i32 187, i32 61, i32 251, i32 76, i32 123, i32 2,
	i32 209, i32 221, i32 250, i32 248, i32 221, i32 6, i32 193, i32 206,
	i32 17, i32 79, i32 196, i32 133, i32 247, i32 85, i32 12, i32 34,
	i32 121, i32 255, i32 216, i32 176, i32 208, i32 87, i32 18, i32 242,
	i32 215, i32 73, i32 258, i32 97, i32 182, i32 168, i32 212, i32 84,
	i32 188, i32 192, i32 251, i32 157, i32 36, i32 154, i32 178, i32 147,
	i32 56, i32 115, i32 193, i32 239, i32 238, i32 37, i32 117, i32 0,
	i32 186, i32 14, i32 149, i32 43, i32 184, i32 100, i32 254, i32 171,
	i32 16, i32 233, i32 48, i32 109, i32 99, i32 226, i32 27, i32 130,
	i32 29, i32 234, i32 130, i32 44, i32 202, i32 207, i32 152, i32 8,
	i32 237, i32 180, i32 134, i32 42, i32 255, i32 180, i32 33, i32 261,
	i32 46, i32 146, i32 222, i32 203, i32 140, i32 63, i32 134, i32 48,
	i32 163, i32 191, i32 213, i32 238, i32 46, i32 167, i32 176, i32 212,
	i32 18, i32 8, i32 200, i32 233, i32 126, i32 60, i32 144, i32 217,
	i32 153, i32 145, i32 253, i32 250, i32 128, i32 252, i32 163, i32 165,
	i32 204, i32 183, i32 26, i32 215, i32 84, i32 129, i32 103, i32 151,
	i32 244, i32 54, i32 165, i32 170, i32 133, i32 37, i32 240, i32 22,
	i32 114, i32 92, i32 50, i32 61, i32 124, i32 85, i32 129, i32 166,
	i32 169, i32 232, i32 205, i32 218, i32 4, i32 214, i32 173, i32 2,
	i32 223, i32 118, i32 260, i32 185, i32 19, i32 91, i32 66, i32 30,
	i32 210, i32 197, i32 60, i32 113, i32 215, i32 32, i32 130, i32 162,
	i32 195, i32 143, i32 156, i32 17, i32 77, i32 75, i32 15, i32 172,
	i32 87, i32 178, i32 251, i32 126, i32 224, i32 196, i32 220, i32 34,
	i32 120, i32 141, i32 124, i32 108, i32 258, i32 227, i32 240, i32 192,
	i32 54, i32 47, i32 28, i32 142, i32 148, i32 150, i32 35, i32 177,
	i32 77, i32 164, i32 1, i32 225, i32 162, i32 12, i32 158, i32 154,
	i32 78, i32 105, i32 114, i32 182, i32 190, i32 66, i32 67, i32 243,
	i32 45, i32 111, i32 7, i32 189, i32 55, i32 185, i32 65, i32 200,
	i32 20, i32 111, i32 103, i32 63, i32 145, i32 183, i32 7, i32 173,
	i32 50, i32 243, i32 117, i32 144, i32 169, i32 82, i32 115, i32 224,
	i32 17, i32 74, i32 91, i32 89, i32 122, i32 236, i32 187, i32 137,
	i32 156, i32 108, i32 11, i32 92, i32 31, i32 138, i32 235, i32 40,
	i32 261, i32 141, i32 254, i32 256, i32 25, i32 176, i32 74, i32 212,
	i32 260, i32 27, i32 68, i32 90, i32 97, i32 115, i32 31, i32 106,
	i32 214, i32 37, i32 73, i32 246, i32 110, i32 125, i32 189, i32 89,
	i32 88, i32 95, i32 131, i32 224, i32 236, i32 257, i32 232, i32 200,
	i32 197, i32 228, i32 166, i32 132, i32 241, i32 229, i32 10, i32 49,
	i32 93, i32 153, i32 63, i32 138, i32 153, i32 62, i32 119, i32 139,
	i32 248, i32 86, i32 162, i32 238, i32 146, i32 84, i32 76, i32 71,
	i32 188, i32 138, i32 198, i32 127, i32 257, i32 54, i32 112, i32 132,
	i32 90, i32 23, i32 75, i32 131, i32 31, i32 74, i32 219, i32 161,
	i32 23, i32 4, i32 173, i32 178, i32 125, i32 201, i32 227, i32 116,
	i32 175, i32 32, i32 3, i32 167, i32 239, i32 30, i32 19, i32 228,
	i32 218, i32 95, i32 36, i32 5, i32 191, i32 252, i32 158, i32 235,
	i32 245, i32 196, i32 241, i32 259, i32 78, i32 64, i32 150, i32 194,
	i32 123, i32 136, i32 237, i32 246, i32 102, i32 39, i32 183, i32 142,
	i32 69, i32 26, i32 77, i32 80, i32 217, i32 24, i32 155, i32 38,
	i32 225, i32 191, i32 135, i32 227, i32 105, i32 244, i32 57, i32 168,
	i32 93, i32 62, i32 134, i32 46, i32 135, i32 204, i32 148, i32 80,
	i32 198, i32 218, i32 157, i32 85, i32 62, i32 98, i32 230, i32 156,
	i32 120, i32 6, i32 15, i32 75, i32 149, i32 52, i32 179, i32 71,
	i32 23, i32 161, i32 128, i32 66, i32 114, i32 216, i32 55, i32 53,
	i32 205, i32 109, i32 137, i32 209, i32 217, i32 82, i32 213, i32 131,
	i32 65, i32 155
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
!2 = !{!".NET for Android remotes/origin/release/10.0.1xx @ db080b91dbe7947a69005a203ea98d9bb207ad04"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
