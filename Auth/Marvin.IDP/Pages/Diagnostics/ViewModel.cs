// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Buffers.Text;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.Text.Json;

namespace IdentityServerHost.Pages.Diagnostics;

public class ViewModel
{
    public ViewModel(AuthenticateResult result)
    {
        AuthenticateResult = result;

        Clients = Enumerable.Empty<string>();

        if (result?.Properties?.Items.TryGetValue("client_list", out var encoded) == true)
        {
            if (!string.IsNullOrWhiteSpace(encoded))
            {
                try
                {
                    var maxLen = Base64.GetMaxDecodedFromUtf8Length(encoded.Length);
                    var buffer = new byte[maxLen];
                    if (!Base64Url.TryDecodeFromChars(encoded, buffer, out var bytesWritten))
                    {
                        Clients = Enumerable.Empty<string>();
                        return;
                    }

                    var value = Encoding.UTF8.GetString(buffer, 0, bytesWritten);
                    Clients = JsonSerializer.Deserialize<string[]>(value) ?? Enumerable.Empty<string>();
                }
                catch (FormatException)
                {
                    Clients = Enumerable.Empty<string>();
                }
                catch (JsonException)
                {
                    Clients = Enumerable.Empty<string>();
                }
            }
        }
    }

    public AuthenticateResult AuthenticateResult { get; }
    public IEnumerable<string> Clients { get; }
}