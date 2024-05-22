﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NuGetGallery
{
    public interface ICloudBlobContainer
    {
        Task CreateIfNotExistAsync(bool enablePublicAccess);
        ISimpleCloudBlob GetBlobReference(string blobAddressUri);
        Task<bool> ExistsAsync(CloudBlobLocationMode? cloudBlobLocationMode);
        Task<bool> DeleteIfExistsAsync();
        Task CreateAsync(bool enablePublicAccess);
        Task<ISimpleBlobResultSegment> ListBlobsSegmentedAsync(
            string prefix,
            bool useFlatBlobListing,
            BlobListingDetails blobListingDetails,
            int? maxResults,
            IBlobListContinuationToken blobContinuationToken,
            TimeSpan? requestTimeout,
            CloudBlobLocationMode? cloudBlobLocationMode,
            CancellationToken cancellationToken);
    }
}
