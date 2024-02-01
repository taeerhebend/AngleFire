using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirestoreController : ControllerBase
    {
        private FirestoreDb firestoreDb;

        public FirestoreController()
        {
            // Initialize Firestore with your project ID
            string projectId = "your-project-id"; // Replace with your Firestore project ID
            firestoreDb = FirestoreDb.Create(projectId);
        }

        [HttpGet("chatGptBot/{documentId}")]
        public async Task<IActionResult> GetChatGptBotDocument(string documentId)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("firestore-chatgpt-bot-2").Document(documentId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("palmSummarizeText/{documentId}")]
        public async Task<IActionResult> GetPaLMSummarizeTextDocument(string documentId)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("firestore-palm-summarize-text").Document(documentId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("multimodalGenai/{documentId}")]
        public async Task<IActionResult> GetMultimodalGenaiDocument(string documentId)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("firestore-multimodal-genai").Document(documentId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("genaiChatbot/{documentId}")]
        public async Task<IActionResult> GetGenaiChatbotDocument(string documentId)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("firestore-genai-chatbot").Document(documentId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("palmGenText/{documentId}")]
        public async Task<IActionResult> GetPaLMGenTextDocument(string documentId)
        {
            try
            {
                DocumentReference docRef = firestoreDb.Collection("firestore-palm-gen-text").Document(documentId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> data = snapshot.ToDictionary();
                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // Add more methods for other Firestore extensions as needed
    }

[HttpGet("multimodalGenai/{documentId}")]
public async Task<IActionResult> GetMultimodalGenaiDocument(string documentId)
{
    try
    {
        DocumentReference docRef = firestoreDb.Collection("firestore-multimodal-genai").Document(documentId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Dictionary<string, object> data = snapshot.ToDictionary();
            return Ok(data);
        }
        else
        {
            return NotFound();
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error: {ex.Message}");
    }
}

[HttpGet("genaiChatbot/{documentId}")]
public async Task<IActionResult> GetGenaiChatbotDocument(string documentId)
{
    try
    {
        DocumentReference docRef = firestoreDb.Collection("firestore-genai-chatbot").Document(documentId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Dictionary<string, object> data = snapshot.ToDictionary();
            return Ok(data);
        }
        else
        {
            return NotFound();
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error: {ex.Message}");
    }
}

[HttpGet("palmGenText/{documentId}")]
public async Task<IActionResult> GetPaLMGenTextDocument(string documentId)
{
    try
    {
        DocumentReference docRef = firestoreDb.Collection("firestore-palm-gen-text").Document(documentId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            Dictionary<string, object> data = snapshot.ToDictionary();
            return Ok(data);
        }
        else
        {
            return NotFound();
        }
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error: {ex.Message}");
    }
}

        // Add more methods for other Firestore extensions as needed
    }
}

}